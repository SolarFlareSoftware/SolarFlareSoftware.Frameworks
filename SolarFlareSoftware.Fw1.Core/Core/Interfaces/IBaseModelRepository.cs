using SolarFlareSoftware.Fw1.Core.Events;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IBaseModelRepository<T>: IBaseRepository where T : IBaseModel
    {
        RepositoryPreSaveEventArgs<T> SignalPreSaveEventHandlers(T model);
    }
}
