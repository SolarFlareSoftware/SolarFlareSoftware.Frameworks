using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Repository.EF.Context
{
    public class EFContext : DbContext, IDatabaseContext
    {
        public string ContextID
        {
            get { return this.ContextId.ToString(); }
        }
        public IDbContextTransaction Transaction { get; set; } = null;
        public ILogger<EFContext> Logger { get; set; }
        public IPrincipal Principal { get; set; }

        #region DBSets
        public virtual DbSet<BridgeDefinition> BridgeDefinitions { get; set; }
        public virtual DbSet<BridgeDefinitionFile> BridgeDefinitionFiles { get; set; }
        public virtual DbSet<DashboardAnalysisLog> DashboardAnalysisLogs { get; set; }
        public virtual DbSet<DashboardPermitLog> DashboardPermitLogs { get; set; }
        public virtual DbSet<Interchange> Interchanges { get; set; }
        public virtual DbSet<InterchangeGore> InterchangeGores { get; set; }
        public virtual DbSet<InterchangePoint> InterchangePoints { get; set; }
        public virtual DbSet<BroadcastMessage> BroadcastMessages { get; set; }
        public virtual DbSet<BroadcastMessageHistory> BroadcastMessageHistories { get; set; }
        public virtual DbSet<BroadcastMessageMode> BroadcastMessageModes { get; set; }
        public virtual DbSet<BroadcastMessageType> BroadcastMessageTypes { get; set; }
        #endregion DBSets

        public EFContext(DbContextOptions<EFContext> options, IPrincipal principal, ILogger<EFContext> logger) : base(options)
        {
            var relationalOptions = RelationalOptionsExtension.Extract(options);
            relationalOptions.WithMigrationsHistoryTableName("EFMigrations");
            relationalOptions.WithMigrationsHistoryTableSchema("ef");
            Principal = principal;
            Logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("BridgesDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTime CreatedToUse = new DateTime(2021, 7, 20, 14, 23, 39, 947, DateTimeKind.Local);
            DateTime CreatedToUseForWayInThePast = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<BridgeDefinition>();
            modelBuilder.Entity<BridgeDefinitionFile>();
            modelBuilder.Entity<DashboardAnalysisLog>();
            modelBuilder.Entity<DashboardPermitLog>();
            modelBuilder.Entity<Interchange>(e =>
            {
                e.ToTable("tblInterchange");
            });
            modelBuilder.Entity<InterchangeGore>();
            modelBuilder.Entity<InterchangePoint>();
            modelBuilder.Entity<BroadcastMessage>();
            modelBuilder.Entity<BroadcastMessageHistory>();
            modelBuilder.Entity<BroadcastMessageMode>();
            modelBuilder.Entity<BroadcastMessageType>();
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

        public bool InitiateTransaction()
        {
            Transaction = Database.BeginTransaction();
            if (Transaction != null)
            {
                return true;
            }
            else
                return false;
        }

        public bool Save()
        {
            bool saveSuccessful = false;
            try
            {
                DateTime now = DateTime.Now;
                var changes = this.ChangeTracker.Entries();
                foreach (var entityEntry in changes)
                {
                    if (entityEntry.Entity is IAuditableFull && entityEntry.State != EntityState.Unchanged)
                    {

                        entityEntry.CurrentValues["LastModified"] = now;
                        entityEntry.CurrentValues["ModifiedBy"] = Principal == null ? "System" : Principal.Identity.Name;
                        if (entityEntry.State == EntityState.Added)
                        {
                            entityEntry.CurrentValues["Created"] = now;
                            entityEntry.CurrentValues["CreatedBy"] = Principal == null ? "System" : Principal.Identity.Name;
                        }
                    }
                }

                var saveResult = SaveChanges();

                if (saveResult > 0)
                {
                    saveSuccessful = true;
                }
            }
            catch (ObjectDisposedException exObjDisposed)
            {
                //Logger.LogError(exObjDisposed, "Error in EFContext.Save");
            }
            catch (InvalidOperationException exInvalidOp)
            {
                //Logger.LogError(exInvalidOp, "Error in EFContext.Save");
            }
            catch (DbUpdateConcurrencyException exConcurrency)
            {
                // TODO: determine the procedure for handling concurrency exceptions.
                //Logger.LogError(exConcurrency, "Error in EFContext.Save");
            }
            catch (NotSupportedException exNotSupported)
            {
                //Logger.LogError(exNotSupported, "Error in EFContext.Save");
            }
            catch (DbUpdateException ex)
            {
                //Logger.LogError(ex, "Error in EFContext.Save");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error in EFContext.Save");
            }

            return saveSuccessful;
        }
    }
}
