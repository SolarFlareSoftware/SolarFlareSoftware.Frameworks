using System;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IQuerySortOrder<T>
    {
        SortOrderDirectionEnum DirectionIndicator { get; set; }
        Expression<Func<T, object>> OrderedProperty { get; set; }
    }
}