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
