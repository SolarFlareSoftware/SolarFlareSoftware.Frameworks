using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using System;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class ServicePreSaveNotificationEventArgs<T> : EventArgs where T : IBaseModel    {
        public bool CancelSave 
        {
            get { return RepositoryEventArgs.CancelSave; }
            set { RepositoryEventArgs.CancelSave = value; } 
        }
        public short Action { get; set; }
        public string? ActionBy { get; set; }
        public IBaseModel EntityBeingSaved { get; private set; }
        public IRepository<T> RepositoryBeingUsed { get; private set; }
        public RepositoryPreSaveEventArgs<T> RepositoryEventArgs { get; private protected set; }
        public ServicePreSaveNotificationEventArgs(RepositoryPreSaveEventArgs<T> e, IBaseModel entityBeingSaved, IRepository<T> repositoryBeingUsed, short action)
        {
            RepositoryBeingUsed = repositoryBeingUsed;
            EntityBeingSaved = entityBeingSaved;
            RepositoryEventArgs = e;
            Action = action;
        }
        public ServicePreSaveNotificationEventArgs(RepositoryPreSaveEventArgs<T> e, IBaseModel entityBeingSaved, IRepository<T> repositoryBeingUsed, short action, string actionBy)
        {
            RepositoryBeingUsed = repositoryBeingUsed;
            EntityBeingSaved = entityBeingSaved;
            RepositoryEventArgs = e;
            Action = action;
            ActionBy = actionBy;
        }
    }
}
