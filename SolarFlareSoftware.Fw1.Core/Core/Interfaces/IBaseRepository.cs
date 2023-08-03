using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Events;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IBaseRepository
    {
        IDatabaseContext DatabaseContext { get; }
        bool InTransaction { get; set; }
        IDatabaseActionResult SaveChanges(bool inTransaction = false);
        bool HasChanges();
        string ModelType();
        void SignalSaveEventHandlers(bool saveSuccessful);
    }
}
