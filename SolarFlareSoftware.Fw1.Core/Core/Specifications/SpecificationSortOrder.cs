﻿/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
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
