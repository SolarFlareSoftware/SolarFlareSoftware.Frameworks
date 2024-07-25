//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Specifications
{
    public class SpecificationSortOrder<T> : IQuerySortOrder<T>
    {
        /// <summary>
        /// this is the properth on which this directive's ordering will occur
        /// </summary>
        public Expression<Func<T, object>>? OrderedProperty { get; set; }
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
