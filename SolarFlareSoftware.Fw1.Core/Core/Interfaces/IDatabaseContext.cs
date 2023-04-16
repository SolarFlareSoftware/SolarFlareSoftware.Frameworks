using System.Data.Common;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IDatabaseContext
    {
        bool Save();
        bool InitiateTransaction();
        void AbandonTransaction();
        void CompleteTransaction();
        DbConnection GetDatabaseConnection();
        string ContextID { get; }
    }
}
