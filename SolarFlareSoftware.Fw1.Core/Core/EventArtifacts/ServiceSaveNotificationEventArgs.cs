//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class ServiceSaveNotificationEventArgs : EventArgs
    {
        public bool SaveSuccessful { get; private set; }
        public IBaseModel EntityBeingSaved { get; private set; }
        public string? ActionBy { get; private set; }
        public ServiceSaveNotificationEventArgs(bool saveSuccessful, IBaseModel entityBeingSaved)
        {
            SaveSuccessful = saveSuccessful;
            EntityBeingSaved = entityBeingSaved;
        }
        public ServiceSaveNotificationEventArgs(bool saveSuccessful, IBaseModel entityBeingSaved, string actionBy)
        {
            SaveSuccessful = saveSuccessful;
            EntityBeingSaved = entityBeingSaved;
            this.ActionBy = actionBy;
        }
    }
}
