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

        public override bool IsSatisfiedBy(T entity)
        {
            bool s1IsSatisfiedBy = _specifications[0].IsSatisfiedBy(entity);
            bool s2IsSatisfiedBy = _specifications[1].IsSatisfiedBy(entity);

            string tmpErrorMessage = "";

            // only build an error message for this test if an overarching error message has not already been defined by the system
            if (this.SpecificationErrorMessage.Length == 0)
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

            return s1IsSatisfiedBy && s2IsSatisfiedBy;
        }
    }
}
