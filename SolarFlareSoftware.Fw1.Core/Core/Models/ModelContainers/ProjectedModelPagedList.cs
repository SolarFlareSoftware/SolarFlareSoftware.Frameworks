//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    public class ProjectedModelPagedList
    {
        public IList<IProjectedModel>? ModelList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get { return ModelList == null ? 0: ModelList.Count; } }
        // this property is used to contain the total count of IAncillaryModels if paging were not applied.
        public int ModelPopulationCount { get; set; }
    }
}
