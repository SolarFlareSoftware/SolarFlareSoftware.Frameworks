using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventArgs<T> : EventArgs where T : IBaseModel
    {
        public T Model { get; }
        public RepositoryPreSaveEventArgs(T model)
        {
            Model = model;
        }
        public bool CancelSave { get; set; } = false;
    }
}
