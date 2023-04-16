using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;

namespace SolarFlareSoftware.Fw1.Core.ServiceInterfaces
{
    public interface IBroadcastMessageTypeService
    {
        IRepository<BroadcastMessageType> Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public BaseModelPagedList<BroadcastMessageType> GetActiveBroadcastMessageTypes();

    }
}