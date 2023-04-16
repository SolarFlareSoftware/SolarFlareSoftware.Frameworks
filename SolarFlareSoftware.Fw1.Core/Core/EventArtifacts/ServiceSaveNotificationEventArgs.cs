using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class ServiceSaveNotificationEventArgs : EventArgs
    {
        public bool SaveSuccessful { get; private set; }
        public IBaseModel EntityBeingSaved { get; private set; }
        public string ActionBy { get; private set; }
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
