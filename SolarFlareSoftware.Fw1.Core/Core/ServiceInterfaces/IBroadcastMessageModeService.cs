using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;

namespace SolarFlareSoftware.Fw1.Core.ServiceInterfaces
{
    public interface IBroadcastMessageModeService
    {
        IRepository<BroadcastMessageMode> Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public BaseModelPagedList<BroadcastMessageMode> GetActiveBroadcastMessageModes();
    }
}