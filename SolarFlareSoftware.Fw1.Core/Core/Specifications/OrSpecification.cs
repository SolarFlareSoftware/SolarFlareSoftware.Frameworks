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
        private readonly List<ISpecification<T>> _specifications = null;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _specifications = new List<ISpecification<T>>();
            _specifications.Add(left);
            _specifications.Add(right);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), "s");

            var body = _specifications.Select(exp => exp.ToExpression().Body)
                .Select(exp => ParameterReplacer.Replace(param, exp))
                .Aggregate((left, right) => System.Linq.Expressions.Expression.Or(left, right));

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);

        }
    }
}
