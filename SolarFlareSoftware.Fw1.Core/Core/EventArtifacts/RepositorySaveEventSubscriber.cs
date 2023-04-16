using SolarFlareSoftware.Fw1.Core.Interfaces;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositorySaveEventSubscriber<T> where T : IBaseModel
    {
        private IBaseModel Entity;
        IRepository<T> Repository;
        public string ActionBy;
        public event EventHandler<ServiceSaveNotificationEventArgs> SaveNotificationEvent;

        public RepositorySaveEventSubscriber(IRepository<T> repository, IBaseModel entity )
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositorySaveEvent += Repository_RepositorySaveEvent;
        }

        public RepositorySaveEventSubscriber(IRepository<T> repository, IBaseModel entity, string actionBy)
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositorySaveEvent += Repository_RepositorySaveEvent;
            ActionBy = actionBy;
        }

        ~RepositorySaveEventSubscriber()
        {
            Repository.RepositorySaveEvent -= Repository_RepositorySaveEvent;
        }

        public void Repository_RepositorySaveEvent(object sender, RepositorySaveEventArgs e)
        {
            // this is a safety check to deal with potential unsubscribes happening at an inopportune time
            EventHandler<ServiceSaveNotificationEventArgs> saveEvent = SaveNotificationEvent;
            if (saveEvent != null)
            {
                ServiceSaveNotificationEventArgs args = new ServiceSaveNotificationEventArgs(e.SaveSucceeded, Entity, ActionBy);
                saveEvent(this, args);
            }
        }
    }
}
