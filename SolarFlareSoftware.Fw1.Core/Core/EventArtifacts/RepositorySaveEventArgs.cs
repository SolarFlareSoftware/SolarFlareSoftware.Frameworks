using System;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositorySaveEventArgs : EventArgs
    {
        public bool SaveSucceeded { get; set; }
        public RepositorySaveEventArgs(bool saveSucceeded)
        {
            SaveSucceeded = saveSucceeded;
        }
    }
}
