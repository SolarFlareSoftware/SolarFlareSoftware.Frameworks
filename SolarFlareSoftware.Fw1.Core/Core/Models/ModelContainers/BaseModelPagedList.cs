//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    public class BaseModelPagedList<T> : IBaseModelPagedList<T> where T : IBaseModel
    {
        public IList<T>? EntityList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        // this is the count of records that are contained in EntityList. This may differ from EntityPopulationCount in the case of paging results
        public int Count { get { return EntityList == null ? 0 : EntityList.Count; } }
        // this property is used to contain the total count of DomainEntities if paging were not applied.
        public int EntityPopulationCount { get; set; }
    }
}
