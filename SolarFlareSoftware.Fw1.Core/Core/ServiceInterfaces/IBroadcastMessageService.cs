using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;

namespace SolarFlareSoftware.Fw1.Core.ServiceInterfaces
{
    public interface IBroadcastMessageService
    {
        IRepository<BroadcastMessage> Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        BaseModelPagedList<BroadcastMessage> GetActiveBannerMessages();
        BaseModelPagedList<BroadcastMessage> GetActiveWidgetMessages();
        BaseModelPagedList<BroadcastMessage> GetAllMessages(int page = 1, int pageSize = 20, string sortColumn = "Created", SortOrderDirectionEnum sortDirection = SortOrderDirectionEnum.none);
        BroadcastMessage GetById(int broadcastMessageID);
        BroadcastMessage SaveBroadcastMessage(BroadcastMessage broadcastMessage);
        BroadcastMessage DeleteBroadcastMessage(int broadcastMessageID);
    }
}