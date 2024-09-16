/*
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
using SolarFlareSoftware.Fw1.Core.Core.Exceptions;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Specifications
{
    /// <summary>
    /// This specialized BaseSpecification{T} object takes in two ISpecification{T} objects and calls to IsSatisfiedBy must satisfy the Expression 
    /// of either of the specs to get a postive result.
    /// </summary>
    /// <typeparam name="T">any type where T is a BaseModel</typeparam>
    public class OrSpecification<T> : BaseSpecification<T>
    {
        private readonly List<ISpecification<T>>? _specifications = null;
        protected string _orGrpErrorMsgOverride = string.Empty;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _specifications = new List<ISpecification<T>>();
            _specifications.Add(left);
            _specifications.Add(right);
        }

        public void SetOverrideErrorMsg(string msg)
        {
            _orGrpErrorMsgOverride = msg;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            // guard clause. this will raise a known exception
            if (_specifications == null || _specifications.Count == 0) throw new SpecificationExpressionNotDefinedException("Or");

            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), "s");

            var body = _specifications.Select(exp => exp.ToExpression().Body)
                .Select(exp => ParameterReplacer.Replace(param, exp))
                .Aggregate((left, right) => System.Linq.Expressions.Expression.Or(left, right));

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);
        }

        public override bool IsSatisfiedBy(T entity)
        {
            // guard clause. this will raise a known exception
            if (_specifications == null || _specifications.Count == 0) throw new SpecificationExpressionNotDefinedException("Or");

            bool s1IsSatisfiedBy = _specifications![0].IsSatisfiedBy(entity);
            bool s2IsSatisfiedBy = _specifications[1].IsSatisfiedBy(entity);

            string tmpErrorMessage = "";

            // only display the 'AND group' error message if the "override msg" is empty. otherwise, just display the "override msg".
            if (_orGrpErrorMsgOverride == string.Empty)
            {
                if (!s1IsSatisfiedBy && !s2IsSatisfiedBy)
                {
                    if (_specifications[0].SpecificationErrorMessage.Length > 0 && _specifications[1].SpecificationErrorMessage.Length > 0)
                        tmpErrorMessage = $"{_specifications[0].SpecificationErrorMessage} or {_specifications[1].SpecificationErrorMessage}";
                }

                if (tmpErrorMessage.Length > 0)
                {
                    this.SpecificationErrorMessage = $"Condition not satisfied: {tmpErrorMessage}";
                } 
            }
            else
            {
                SpecificationErrorMessage = _orGrpErrorMsgOverride;
            }

            return (s1IsSatisfiedBy == true || s2IsSatisfiedBy == true);
        }
    }
}
