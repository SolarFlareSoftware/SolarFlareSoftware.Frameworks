/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
using Microsoft.Extensions.Logging;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Repository.EF
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _inTransaction = false;
        public bool InTransaction { get => _inTransaction; }

        private List<IBaseRepository> EnlistedRepositories { get; set; }
        private ILogger? Logger { get; set; }

        public UnitOfWork(ILogger<UnitOfWork> logger)
        {
            EnlistedRepositories = new List<IBaseRepository>();
            Logger = logger;
        }

        public UnitOfWork()
        {
            EnlistedRepositories = new List<IBaseRepository>();
        }

        public bool BeginTransaction()
        {
            if (!InTransaction)
            {
                if(EnlistedRepositories?.Count > 0)
                {
                    foreach(IBaseRepository repo in EnlistedRepositories)
                    {
                        repo.DatabaseContext.InitiateTransaction();
                        repo.InTransaction = true;
                    }
                    _inTransaction = true;
                }
                else
                {
                    _inTransaction = false;
                }
            }
            return InTransaction;
        }

        public bool BeginTransaction(IBaseRepository repository)
        {
            bool inTransaction;
            JoinTransaction(repository);
            if (InTransaction)
            {
                inTransaction = true;
                // the Unit of Work's transaction is already in progress, so only the new database needs to establish a Transaction condition
                repository.DatabaseContext.InitiateTransaction();
            }
            else
            {
                inTransaction = BeginTransaction();
            }

            repository.InTransaction = inTransaction;

            return inTransaction;
        }

        /// <summary>
        /// This will add a database context to the collection of contexts using this as a unit of work and set the repository's InTransaction flag 
        /// to 'true'. NOTE: this is an entity framework-specific implementation of IUnitOfWork and will therefore require the EF database context
        /// </summary>
        /// <param name="repository">An IBaseRepository object</param>
        /// <returns></returns>
        public bool JoinTransaction(IBaseRepository repository)
        {
            int pre = EnlistedRepositories.Count;
            int post = 0;

            EnlistedRepositories.Add(repository);
            repository.InTransaction = true;
            post = EnlistedRepositories.Count;
            return post > pre;
        }

        public bool Complete()
        {
            bool committed = false;
            bool saveFailed = false;
            string modelType = string.Empty;

            foreach(IBaseRepository repo in EnlistedRepositories)
            {
                if(repo.HasChanges())
                {
                    modelType = repo.ModelType();
                    var saveResult = repo.SaveChanges();
                    if (!saveResult.Succeeded)
                    {
                        saveFailed = true;
                        break;
                    }
                }
            }

            if(saveFailed)
            {
                Logger?.LogError(string.Format("Error in UnitOfWork.Complete involving a {0} record", modelType));
            }
            else
            {
                if (InTransaction)
                {
                    List<string> contextIDs = new List<string>();

                    foreach (IBaseRepository repo in EnlistedRepositories)
                    {
                        if (repo.InTransaction)
                        {
                            try
                            {
                                modelType = repo.ModelType();
                                if (!contextIDs.Contains(repo.DatabaseContext.ContextID))
                                {
                                    contextIDs.Add(repo.DatabaseContext.ContextID);
                                    repo.DatabaseContext.CompleteTransaction();
                                }
                                committed = true;
                            }
                            catch (Exception ex)
                            {
                                Logger?.LogError(ex, string.Format("Error in UnitOfWork.Complete trying to commit the transaction related to a {0} record)", modelType));
                                committed = false;
                                throw;
                            }
                        }
                        repo.InTransaction = false;
                    }
                }
            }
            
            if(committed)
            {
                _inTransaction = false;
                EnlistedRepositories.Clear();
            }
            return committed;
        }

        public void Rollback()
        {
            if (InTransaction)
            {
                string modelType = string.Empty;
                foreach (IBaseRepository repo in EnlistedRepositories)
                {
                    modelType = repo.ModelType();
                    try
                    {
                        repo.DatabaseContext.AbandonTransaction();
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, string.Format("Error in UnitOfWork.Rollback trying to rollback the transaction involving a {0} record)", modelType));
                        throw;
                    }
                }
            }
        }

        //private bool disposed = false;

        //public void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
                    
        //        }
        //    }
        //    disposed = true;
        //}

        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
