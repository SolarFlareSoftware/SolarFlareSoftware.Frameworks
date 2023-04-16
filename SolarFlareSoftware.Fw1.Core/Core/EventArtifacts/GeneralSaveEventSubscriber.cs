using SolarFlareSoftware.Fw1.Core.Interfaces;

namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class GeneralSaveEventSubscriber<T> : RepositorySaveEventSubscriber<T> where T : IBaseModel
    {
        public short Action;
        public GeneralSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action)
            : base(repository, entity)
        {
            Action = action;
        }
        public GeneralSaveEventSubscriber(IRepository<T> repository, IBaseModel entity, short action, string actionBy)
            : base(repository, entity, actionBy)
        {
            Action = action;
            ActionBy = actionBy;
        }
    }
}
