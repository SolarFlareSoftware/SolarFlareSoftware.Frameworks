using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Specifications
{
    /// <summary>
    /// This class is used to create a new specification object whose Expression is an inversion of the Expression of the ISpecification{T} 
    /// object passed to the constructor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotSpecification<T> : BaseSpecification<T>
    {
        private ISpecification<T> originalSpec;

        public NotSpecification(ISpecification<T> spec)
        {
            originalSpec = spec;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return originalSpec.IsSatisfiedBy(entity) == false;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> originalTree = this.originalSpec.ToExpression();

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.Not(originalTree.Body),
                originalTree.Parameters.Single()
            );
        }
    }
}
