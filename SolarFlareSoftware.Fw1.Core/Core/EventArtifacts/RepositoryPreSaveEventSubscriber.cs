/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
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
