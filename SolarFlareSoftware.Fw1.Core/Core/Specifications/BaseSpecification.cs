using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SolarFlareSoftware.Fw1.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        // this is the "base" concept around which the ISpecification revolves. It is basically a test for a condition. This could be a simple or compound condition.
        protected Expression<Func<T, bool>> Expression = null;
        // this property allows one to categorize a validation error that may occur as part of the condition test
        public string SpecificationErrorKeyOrCategory { get; set; }
        // a validation error that occured as part of the condition test
        public string SpecificationErrorMessage { get; set; }

        /// <summary>
        /// This method returns the object's Expression that can be used in various LINQ applications
        /// </summary>
        /// <returns>an Expression{Func{T, bool}}</returns>
        public virtual Expression<Func<T, bool>> ToExpression()
        {
            return this.Expression;
        }
        /// <summary>
        /// This is a collection of LINQ Extension statements that indicate one or more child tables to retrieve along with this object's base type (type T). All tables in this collection will be returned from the 
        /// DbContext when the command is executed
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = null;
        public List<Expression<Func<T, object>>> LeftJoins { get; } = null;
        /// <summary>
        /// This is a collection of strings that define a "path" of Navigation Properties related to this object's base type (type T). This collection is a little more powerful than 
        /// <seealso cref="Includes"/> due to the fact that one can more easily get many levels deep using a chain of Navigation Properties versus using LINQ extensions
        /// </summary>
        public List<string> NavigationPropertyIncludes { get; } = null;
        public List<string> NavigationPropertyLeftJoins { get; } = null;
        /// <summary>
        /// This is a collection of objects that allows for multiple "order by" directives to be applied to the final IQueryable{T}. The QueryResultOrderDirectives allows one to indicate 
        /// both the property on which to perform the ordering and the directionality of the ordering (ascending or descendint)
        /// </summary>
        public List<SpecificationSortOrder<T>> SortOrderList { get; private set; }
        public Expression<Func<T, object>> OrderBy { get; private set; } = null;
        public Expression<Func<T, object>> OrderByDescending { get; private set; } = null;
        public Expression<Func<T, object>> GroupBy { get; set; } = null;
        public Expression<Func<T, object>> GroupingProjection { get; set; } = null;

        /// <summary>
        /// Default constructor. This object's collections are newed up herein.
        /// </summary>
        public BaseSpecification()
        {
            if (Includes == null)
            {
                Includes = new List<Expression<Func<T, object>>>();
            }

            if (LeftJoins == null)
            {
                LeftJoins = new List<Expression<Func<T, object>>>();
            }

            if (NavigationPropertyIncludes == null)
            {
                NavigationPropertyIncludes = new List<string>();
            }

            if (NavigationPropertyLeftJoins == null)
            {
                NavigationPropertyLeftJoins = new List<string>();
            }

            if (SortOrderList == null)
            {
                SortOrderList = new List<SpecificationSortOrder<T>>();
            }
        }

        /// <summary>
        /// This parameterized constructor accepts an Expression{Func{T, bool}} as an argument. This object's collections are newed up herein.
        /// </summary>
        /// <param name="expression"></param>
        public BaseSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;

            if (Includes == null)
            {
                Includes = new List<Expression<Func<T, object>>>();
            }

            if(LeftJoins == null)
            {
                LeftJoins= new List<Expression<Func<T, object>>>();
            }

            if (NavigationPropertyIncludes == null)
            {
                NavigationPropertyIncludes = new List<string>();
            }

            if (NavigationPropertyLeftJoins == null)
            {
                NavigationPropertyLeftJoins = new List<string>();
            }

            if (SortOrderList == null)
            {
                SortOrderList = new List<SpecificationSortOrder<T>>();
            }
        }

        public void ChangeExpression(Expression<Func<T, bool>> newExpression)
        {
            this.Expression = newExpression;
        }

        protected virtual void AddQueryResultOrderDirective(SpecificationSortOrder<T> directive)
        {
            SortOrderList.Add(directive);
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        /// <summary>
        /// This is the "workhorse" method required by the Specification pattern
        /// </summary>
        /// <param name="entity">a BaseModel of type T</param>
        /// <returns>if the entity satisfies the Expression, false if not.</returns>
        public virtual bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            var result = (predicate(entity) == true);

            if (result == false && entity is ISupportsValidation)
            {
                if (string.IsNullOrWhiteSpace(SpecificationErrorKeyOrCategory))
                {
                    string expAsString = ToExpression().ToString();
                    int lambdaPos = expAsString.IndexOf("=>");
                    if (lambdaPos >= 0)
                    {
                        int periodPos = expAsString.IndexOf(".", lambdaPos + 1);
                        int spacePos = expAsString.IndexOf(" ", periodPos + 1);
                        if (periodPos > -1 && spacePos > -1 && spacePos > periodPos)
                        {
                            SpecificationErrorKeyOrCategory = expAsString.Substring(periodPos + 1, spacePos - periodPos - 1);
                        }
                        else
                        {
                            SpecificationErrorKeyOrCategory = String.Empty;
                        }
                    }
                }

                if (!((ISupportsValidation)entity).ValidationErrors.ContainsKey(SpecificationErrorKeyOrCategory))
                {
                    ((ISupportsValidation)entity).ValidationErrors.Add(SpecificationErrorKeyOrCategory, SpecificationErrorMessage); 
                }
            }

            return result;
        }

        /// <summary>
        /// Joins this ISpecification{T} object with the one passed to this method to create a "compound" ISpecification{T} object that requires both objects' Expression to be satisfied 
        /// to "pass"
        /// </summary>
        /// <param name="specification">another ISpecification{T} object</param>
        /// <returns>an ISpecification{T} object that now enforces the Expressions of both source ISpecification{T} objects</returns>
        public virtual ISpecification<T> And(ISpecification<T> specification)
        {
            if(specification == null)
            {
                return this;
            }
            else
            {
                return new AndSpecification<T>(this, specification);
            }
        }

        /// <summary>
        /// Joins this ISpecification{T} object with the one passed to this method to create a "compound" ISpecification{T} object that enforces an "or" test using both objects' Expressions
        /// </summary>
        /// <param name="specification">another ISpecification{T} object</param>
        /// <returns>an ISpecification{T} object that now enforces the Expressions of both source ISpecification{T} objects</returns>
        public virtual ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Creates a "not" ISpecification{T} object using the Expression from the ISpecificaton{T} object passed to this function
        /// </summary>
        /// <param name="specification">a valid ISpecification{T} object</param>
        /// <returns>a new ISpecification{T} object that envorces a boolean "not" on the Expression of the ISpecification{T} object pass to this function</returns>
        public virtual ISpecification<T> Not(ISpecification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        #region Used to get the underlying Class property name being tested. WARNING: do not call this for CompositeSpecification
        protected string GetFullPropertyName<TProperty>(Expression<Func<T, TProperty>> exp)
        {
            MemberExpression memberExp;
            if (!TryFindMemberExpression(exp.Body, out memberExp))
            {
                return string.Empty;
            }

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        protected bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // a compiler-created Expression will look similar to "x => Convert(x.Property)", "x => Convert(x.property == 1)", or similar. IsConversion will identify if this is the case
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        protected static bool IsConversion(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked);
        }
        #endregion

        protected class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _param;
            private ParameterReplacer(ParameterExpression param)
            {
                _param = param;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node.Type == _param.Type ? base.VisitParameter(_param) : node;
            }

            public static T Replace<T>(ParameterExpression param, T exp) where T : Expression
            {
                return (T)new ParameterReplacer(param).Visit(exp);
            }
        }

    }
}
