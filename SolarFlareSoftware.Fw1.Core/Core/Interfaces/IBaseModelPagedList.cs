using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IBaseModelPagedList<T> where T : IBaseModel
    {
        int Count { get; }
        IList<T>? EntityList { get; set; }
        int EntityPopulationCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}