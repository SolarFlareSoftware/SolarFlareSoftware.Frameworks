using SolarFlareSoftware.Fw1.Core.Events;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IBaseRepository
    {
        IDatabaseContext DatabaseContext { get; }
        bool InTransaction { get; set; }
        bool SaveChanges(bool inTransaction = false);
        bool HasChanges();
        string ModelType();
        void SignalSaveEventHandlers(bool saveSuccessful);
        RepositoryPreSaveEventArgs SignalPreSaveEventHandlers();
    }
}
