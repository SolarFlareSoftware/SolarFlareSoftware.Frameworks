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
        private readonly List<ISpecification<T>> _specifications = null;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
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
                .Aggregate((left, right) => System.Linq.Expressions.Expression.AndAlso(left, right));

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return _specifications[0].IsSatisfiedBy(entity)
            && _specifications[1].IsSatisfiedBy(entity);
        }
    }
}
