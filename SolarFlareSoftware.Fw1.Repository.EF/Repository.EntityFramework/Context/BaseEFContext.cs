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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.Repository.EF.Context
{
    public partial class BaseEFContext : DbContext, IDatabaseContext
    {
        public string ContextID
        {
            get { return this.ContextId.ToString(); }
        }
        public IDbContextTransaction? Transaction { get; set; } = null;
        public ILogger<IDatabaseContext>? Logger { get; set; }
        public IPrincipal Principal { get; set; }

        public BaseEFContext(IPrincipal principal, ILogger<IDatabaseContext>? logger) 
        {
            Principal = principal;
            Logger = logger;
        }

        protected BaseEFContext(DbContextOptions options, IPrincipal principal, ILogger<IDatabaseContext>? logger) : base(options)
        {
            Principal = principal;
            Logger = logger;
        }

        public void AbandonTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
                if (ChangeTracker.HasChanges())
                {
                    ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                }
            }
        }

        public void CompleteTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public DbConnection GetDatabaseConnection()
        {
            return Database.GetDbConnection();
        }

        public virtual bool InitiateTransaction()
        {
            Transaction = Database.BeginTransaction();
            if (Transaction != null)
            {
                return true;
            }
            else
                return false;
        }

        [DebuggerNonUserCode]
        public virtual IDatabaseActionResult Save()
        {
            DatabaseActionResult result = new();
            try
            {
                var saveResult = SaveChanges();
                result.RecordsAffected = saveResult;
                result.Succeeded = saveResult > 0;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error in BaseEFContext.Save");
                result.Exception = ex;
            }

            return result;
        }
    }
}
