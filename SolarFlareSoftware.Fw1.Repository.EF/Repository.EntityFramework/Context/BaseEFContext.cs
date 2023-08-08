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
        public IDbContextTransaction Transaction { get; set; } = null;
        public ILogger<IDatabaseContext> Logger { get; set; }
        public IPrincipal Principal { get; set; }

        public BaseEFContext(IPrincipal principal, ILogger<IDatabaseContext> logger) 
        {
            Principal = principal;
            Logger = logger;
        }

        protected BaseEFContext(DbContextOptions options, IPrincipal principal, ILogger<IDatabaseContext> logger) : base(options)
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
                Logger.LogError(ex, "Error in BaseEFContext.Save");
                result.Exception = ex;
            }

            return result;
        }
    }
}
