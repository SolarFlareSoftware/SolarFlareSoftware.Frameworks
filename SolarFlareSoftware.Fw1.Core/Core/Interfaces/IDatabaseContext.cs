using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using System.Data.Common;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IDatabaseContext
    {
        IDatabaseActionResult Save();
        bool InitiateTransaction();
        void AbandonTransaction();
        void CompleteTransaction();
        DbConnection GetDatabaseConnection();
        string ContextID { get; }
    }
}
