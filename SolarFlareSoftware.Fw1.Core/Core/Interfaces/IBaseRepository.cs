//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Events;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IBaseRepository
    {
        IDatabaseContext DatabaseContext { get; }
        bool InTransaction { get; set; }
        IDatabaseActionResult SaveChanges();
        bool HasChanges();
        string ModelType();
        void SignalSaveEventHandlers(bool saveSuccessful);
    }
}
