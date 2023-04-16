using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Specifications
{
    public class SpecificationSortOrder<T> : IQuerySortOrder<T>
    {
        /// <summary>
        /// this is the properth on which this directive's ordering will occur
        /// </summary>
        public Expression<Func<T, object>> OrderedProperty { get; set; }
        /// <summary>
        /// this is the direction of the sort (ascending or descending)
        /// </summary>
        public SortOrderDirectionEnum DirectionIndicator { get; set; }

        public SpecificationSortOrder()
        {
        }

        public SpecificationSortOrder(Expression<Func<T, object>> orderedProperty, SortOrderDirectionEnum orderDirection)
        {
            OrderedProperty = orderedProperty;
            DirectionIndicator = orderDirection;
        }
    }
}
