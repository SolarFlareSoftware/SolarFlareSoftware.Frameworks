//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
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
