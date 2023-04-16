using SolarFlareSoftware.Fw1.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        List<Expression<Func<T, object>>> Includes { get; }
        List<Expression<Func<T, object>>> LeftJoins { get; }
        List<string> NavigationPropertyIncludes { get; }
        List<string> NavigationPropertyLeftJoins { get; }
        public List<SpecificationSortOrder<T>> SortOrderList { get; }

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }
        void ChangeExpression(Expression<Func<T, bool>> newExpression);
        bool IsSatisfiedBy(T entity);
        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> Or(ISpecification<T> specification);
        ISpecification<T> Not(ISpecification<T> specification);
    }
}
