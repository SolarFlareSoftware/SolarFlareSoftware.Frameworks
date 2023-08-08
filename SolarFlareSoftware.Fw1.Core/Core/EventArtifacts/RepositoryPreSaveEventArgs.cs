using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventArgs<T> : EventArgs where T : IBaseModel
    {
        public T Model { get; }
        public RepositoryPreSaveEventArgs(T model)
        {
            Model = Model;
        }
        public bool CancelSave { get; set; } = false;
    }
}
