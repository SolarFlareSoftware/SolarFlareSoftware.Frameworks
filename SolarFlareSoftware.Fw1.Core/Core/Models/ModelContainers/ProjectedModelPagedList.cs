using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    public class ProjectedModelPagedList
    {
        public IList<IProjectedModel> ModelList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get { return ModelList.Count; } }
        // this property is used to contain the total count of IAncillaryModels if paging were not applied.
        public int ModelPopulationCount { get; set; }
    }
}
