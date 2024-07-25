//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventSubscriber<T> where T : IBaseModel
    {
        public short Action;
        public string ActionBy = string.Empty;
        private IBaseModel Entity;
        IRepository<T> Repository;
        public event EventHandler<ServicePreSaveNotificationEventArgs<T>>? PreSaveNotificationEvent;

        public RepositoryPreSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action)
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositoryPreSaveEvent += Repository_RepositoryPreSaveEvent!;
            Action = action;
        }

        public RepositoryPreSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action, string actionBy)
        {
            Entity = entity;
            Repository = repository;
            Repository.RepositoryPreSaveEvent += Repository_RepositoryPreSaveEvent!;
            Action = action;
            ActionBy = actionBy;
        }

        private void Repository_RepositoryPreSaveEvent(object sender, RepositoryPreSaveEventArgs<T> e)
        {
            // this is a safety check to deal with potential unsubscribes happening at an inopportune time
            EventHandler<ServicePreSaveNotificationEventArgs<T>>? saveEvent = PreSaveNotificationEvent;
            if (saveEvent != null)
            {
                ServicePreSaveNotificationEventArgs<T>? args = null;
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
