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
