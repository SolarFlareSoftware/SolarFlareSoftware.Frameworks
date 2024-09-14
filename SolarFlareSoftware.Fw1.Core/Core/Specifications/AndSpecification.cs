//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
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
    /// of both specs to get a postive result.
    /// </summary>
    /// <typeparam name="T">any type where T is a BaseModel</typeparam>
    public class AndSpecification<T> : BaseSpecification<T>
    {
        // I am using a List to contain the left and right ISpecification objects because it makes processing easier in ToExpression where we have to use the Visitor pattern to build our lambda
        private readonly List<ISpecification<T>>? _specifications = null;
        protected string _andGrpErrorMsgOverride = string.Empty;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _specifications = new List<ISpecification<T>>();
            _specifications.Add(left);
            _specifications.Add(right);
        }

        public void SetOverrideErrorMsg(string msg)
        {
            _andGrpErrorMsgOverride = msg;
        }

        /// <summary>
        /// Obtains an Expression that represents the the compound requirements of the underlying Specifications 
        /// </summary>
        /// <returns>Expression<Func<T, bool>></returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            // guard clause. this will raise a known exception
            if (_specifications == null || _specifications.Count == 0) throw new SpecificationExpressionNotDefinedException("And");

            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), "s");

            var body = _specifications.Select(exp => exp.ToExpression().Body)
                .Select(exp => ParameterReplacer.Replace(param, exp))
                .Aggregate((left, right) => System.Linq.Expressions.Expression.AndAlso(left, right));

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);
        }

        /// <summary>
        /// Use this function to determine if the requirements of the compound "And" Specification are satisfied. NOTE: if the individual Specifications that were Anded herein 
        /// have both defined a SpecificationErrorMessage, this And Specification will indicate which of the conditions failed.
        /// </summary>
        /// <param name="entity">the object to be tested to see if it satisfies the Specifications validations/requirements</param>
        /// <returns>true if conditions are met, false if not</returns>
        public override bool IsSatisfiedBy(T entity)
        {
            // guard clause. this will raise a known exception
            if (_specifications == null || _specifications.Count == 0) throw new SpecificationExpressionNotDefinedException("And");

            bool s1IsSatisfiedBy = _specifications[0].IsSatisfiedBy(entity); 
            bool s2IsSatisfiedBy = _specifications[1].IsSatisfiedBy(entity);

            string tmpErrorMessage = "";

            // only display the 'AND group' error message if the "override msg" is empty. otherwise, just display the "override msg".
            if(_andGrpErrorMsgOverride == string.Empty)
            {
                if (!s1IsSatisfiedBy && _specifications[0].SpecificationErrorMessage.Length > 0)
                {
                    tmpErrorMessage = _specifications[0].SpecificationErrorMessage;
                }
                if (!s2IsSatisfiedBy && _specifications[1].SpecificationErrorMessage.Length > 0)
                {
                    if (tmpErrorMessage.Length > 0)
                    {
                        tmpErrorMessage += "; ";
                    }

                    tmpErrorMessage += _specifications[1].SpecificationErrorMessage;
                }
                if (tmpErrorMessage.Length > 0)
                {
                    SpecificationErrorMessage += tmpErrorMessage;
                }
            }
            else
            {
                SpecificationErrorMessage = _andGrpErrorMsgOverride;
            }

            return s1IsSatisfiedBy && s2IsSatisfiedBy;
        }
    }
}
