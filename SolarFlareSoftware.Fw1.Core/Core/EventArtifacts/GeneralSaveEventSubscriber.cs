//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
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
