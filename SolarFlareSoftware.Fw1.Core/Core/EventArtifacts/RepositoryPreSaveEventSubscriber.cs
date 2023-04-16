﻿using SolarFlareSoftware.Fw1.Core.Interfaces;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventSubscriber<T> where T : IBaseModel
    {
        public short Action;
        public string ActionBy = string.Empty;
        private IBaseModel Entity;
        IRepository<T> Repository;
        public event EventHandler<ServicePreSaveNotificationEventArgs<T>> PreSaveNotificationEvent;

        public RepositoryPreSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action)
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositoryPreSaveEvent += Repository_RepositoryPreSaveEvent;
            Action = action;
        }

        public RepositoryPreSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action, string actionBy)
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositoryPreSaveEvent += Repository_RepositoryPreSaveEvent;
            Action = action;
            ActionBy = actionBy;
        }

        private void Repository_RepositoryPreSaveEvent(object sender, RepositoryPreSaveEventArgs e)
        {
            // this is a safety check to deal with potential unsubscribes happening at an inopportune time
            EventHandler<ServicePreSaveNotificationEventArgs<T>> saveEvent = PreSaveNotificationEvent;
            if (saveEvent != null)
            {
                ServicePreSaveNotificationEventArgs<T> args = null;
                if (string.Empty == ActionBy)
                {
                    args = new ServicePreSaveNotificationEventArgs<T>(e, Entity, Repository, Action);
                }
                else
                {
                    args = new ServicePreSaveNotificationEventArgs<T>(e, Entity, Repository, Action, ActionBy);
                }
                saveEvent(this, args);
            }
        }
    }
}