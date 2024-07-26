//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using SolarFlareSoftware.Fw1.Core;
using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Events;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using SolarFlareSoftware.Fw1.Core.Specifications;
using SolarFlareSoftware.Fw1.Repository.EF.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Repository.EF
{
    public class BaseEFRepository<T> : IDisposable, IRepository<T> where T : BaseModel
    {
        protected readonly BaseEFContext _dbContext;
        public bool InTransaction { get; set; }
        public IDatabaseContext? DatabaseContext { get { return _dbContext; } }
        protected IModelValidator<T>? Validator { get; set; }
        protected IPrincipal Principal { get; set; }
        public event EventHandler<RepositoryPreSaveEventArgs<T>>? RepositoryPreSaveEvent;
        public event EventHandler<RepositorySaveEventArgs>? RepositorySaveEvent;

        protected ILogger<IRepository<T>>? Logger { get; set; }

        /// <summary>
        /// We DI the database context and an indicator of whether or not it is part of a Transaction. 
        /// </summary>
        /// <param name="dbContext">An instance of IDatabaseContext, specifically an SERContext instance (the BaseEFRepository is Entity Framework Specific, and SERContext is, too.</param>
        /// <param name="inTransaction">a bool indicating if this repository is part of a transaction. If not, the add and update methods will be atomic, performing a Save after changes made to the repository contents.</param>
        public BaseEFRepository(IDatabaseContext dbContext, IPrincipal principal, ILogger<IRepository<T>>? logger, IModelValidator<T>? validator = null, bool inTransaction = false)
        {
            _dbContext = (BaseEFContext)dbContext;
            Validator = validator;
            Principal = principal;
            Logger = logger;
            InTransaction = inTransaction;
        }

        /// <summary>
        /// Before the Save completes, this event is ra
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRaisePreSaveEvent(RepositoryPreSaveEventArgs<T> e)
        {
            // this is a safety check to deal with potential unsubscribes happening at an innopportune time
            EventHandler<RepositoryPreSaveEventArgs<T>> preSaveEvent = RepositoryPreSaveEvent!;
            if (preSaveEvent != null)
            {
                preSaveEvent(this, e);
            }
        }

        protected virtual void OnRaiseSaveEvent(RepositorySaveEventArgs e)
        {
            // this is a safety check to deal with potential unsubscribes happening at an innopportune time
            EventHandler<RepositorySaveEventArgs> saveEvent = RepositorySaveEvent!;
            if (saveEvent != null)
            {
                saveEvent(this, e);
            }
        }

        private IQueryable<T> ExtendQueryWithAllIncludes(ISpecification<T> spec, IQueryable<T> query)
        {
            bool includes = false;
            if (spec.Includes?.Count > 0)
            {
                if (spec.ToExpression() != null)
                {
                    query = spec.Includes.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include)); 
                }
                else
                {
                    query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
                }
                includes = true;
            }
            if (spec.NavigationPropertyIncludes?.Count > 0)
            {
                if (spec.ToExpression() != null)
                {
                    query = spec.NavigationPropertyIncludes.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include)); 
                }
                else
                {
                    query = spec.NavigationPropertyIncludes.Aggregate(query, (current, include) => current.Include(include));
                }
                includes = true;
            }
            if (spec.LeftJoins?.Count > 0)
            {
                if (spec.ToExpression() != null)
                {
                    query = spec.LeftJoins.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include).DefaultIfEmpty()); 
                }
                else
                {
                    query = spec.LeftJoins.Aggregate(query, (current, include) => current.Include(include).DefaultIfEmpty()); 
                }
                includes = true;
            }
            if (spec.NavigationPropertyLeftJoins?.Count > 0)
            {
                if (spec.ToExpression() != null)
                {
                    query = spec.NavigationPropertyLeftJoins.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include).DefaultIfEmpty()); 
                }
                else
                {
                    query = spec.NavigationPropertyLeftJoins.Aggregate(query, (current, include) => current.Include(include).DefaultIfEmpty());
                }
                includes = true;
            }
            if (!includes)
            {
                if (spec.ToExpression() != null)
                {
                    query = query.Where(spec.ToExpression()); 
                }
            }

            return query;
        }

        private IQueryable<T> ExtendQueryWithOnlyIncludes(ISpecification<T> spec, IQueryable<T> query)
        {
            bool includes = false;
            if (spec.Includes?.Count > 0)
            {
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
                includes = true;
            }
            if (spec.NavigationPropertyIncludes?.Count > 0)
            {
                query = spec.NavigationPropertyIncludes.Aggregate(query, (current, include) => current.Include(include));
                includes = true;
            }
            if (spec.LeftJoins?.Count > 0)
            {
                query = spec.LeftJoins.Aggregate(query, (current, include) => current.Include(include).DefaultIfEmpty());
                includes = true;
            }
            if (spec.NavigationPropertyLeftJoins?.Count > 0)
            {
                query = spec.NavigationPropertyLeftJoins.Aggregate(query, (current, include) => current.Include(include).DefaultIfEmpty());
                includes = true;
            }
            if (!includes && spec.ToExpression() != null)
            {
                query = query.Where(spec.ToExpression());
            }

            return query;
        }

        private IQueryable<IProjectedModel> ExtendProjectedQueryWithAllIncludes(IProjection projection, ISpecification<T> spec, IQueryable<T> query)
        {
            if (spec.Includes?.Count > 0)
            {
                if(spec.ToExpression() == null)
                {
                    query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
                }
                else
                {
                    query = spec.Includes.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include)); 
                }
            }
            if (spec.NavigationPropertyIncludes?.Count > 0)
            {
                if (spec.ToExpression() == null)
                {
                    query = spec.NavigationPropertyIncludes.Aggregate(query, (current, include) => current.Include(include));
                }
                else
                {
                    query = spec.NavigationPropertyIncludes.Aggregate(query.Where(spec.ToExpression()), (current, include) => current.Include(include)); 
                }
            }
            IQueryable<IProjectedModel>? amQuery = null;
            if(spec.ToExpression() == null)
            {
                amQuery = query.Select(projection.Projection);
            }
            else
            {
                amQuery = query.Where(spec.ToExpression()).Select(projection.Projection);
            }
            return amQuery;
        }

        #region Dynamic Ordering logic
        // this function will apply all of the SpecificationSortOrder ("order by statements") found in the given specification object to the provided IQueryable{T}
        private IQueryable<T> AddSortOrdersToQuery(IQueryable<T> sourceQuery, List<SpecificationSortOrder<T>> orderBy)
        {
            if (orderBy.Count > 0)
            {
                bool isFirst = true;
                foreach (IQuerySortOrder<T> sortExpression in orderBy)
                {
                    if (isFirst)
                    {
                        sourceQuery = OrderByDynamic(sourceQuery, sortExpression);
                        isFirst = false;
                    }
                    else
                        sourceQuery = ThenByDynamic(sourceQuery, sortExpression);
                }
            }

            return sourceQuery;
        }

        private IQueryable<T> AddSortOrderToQuery(IQueryable<T> sourceQuery, SpecificationSortOrder<T> orderBy)
        {
            if (orderBy != null)
            {
                sourceQuery = OrderByDynamic(sourceQuery, orderBy);
            }

            return sourceQuery;
        }

        // this method is used in the OrderByDynamic method to build an ascending order statement
        private readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods()
                                                    .Where(method => method.Name == "OrderBy")
                                                    .Where(method => method.GetParameters().Length == 2)
                                                    .Single();

        // this method is used in the OrderByDynamic method to build a descending order statement
        private readonly MethodInfo OrderByDescMethod = typeof(Queryable).GetMethods()
                                                                .Where(method => method.Name == "OrderByDescending")
                                                                .Where(method => method.GetParameters().Length == 2)
                                                                .Single();

        // this method is used in the ThenByDynamic method to build an ascending "ThenBy" order statement
        private readonly MethodInfo ThenByMethod = typeof(Queryable).GetMethods()
                                                        .Where(method => method.Name == "ThenBy")
                                                        .Where(method => method.GetParameters().Length == 2)
                                                        .Single();

        // this method is used in the ThenByDynamic method to build a descending "ThenBy" order statement
        private readonly MethodInfo ThenByDescMethod = typeof(Queryable).GetMethods()
                                                        .Where(method => method.Name == "ThenByDescending")
                                                        .Where(method => method.GetParameters().Length == 2)
                                                        .Single();

        // removes any type conversion operation from the provided Expression
        private Expression? RemoveConvert(Expression? expression)
        {
            System.Diagnostics.Debug.Assert(expression != null);

            while ((expression != null) && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }

        // dynamically builds an OrderBy statement and adds it to the IQueryable{T} before returning that IQueryable{T}
        private IQueryable<T> OrderByDynamic(IQueryable<T> source, IQuerySortOrder<T> sortExpression)
        {
            //We need to convert the key selector from Expression<Func<T, object>> to a strongly typed Expression<Func<T, TKey>>
            //in order for Entity Framework to be able to translate it to SQL
            MemberExpression? orderByMemExp = RemoveConvert(sortExpression.OrderedProperty.Body) as MemberExpression;
            ParameterExpression sourceParam = sortExpression.OrderedProperty.Parameters[0];

            LambdaExpression orderByLamda = Expression.Lambda(orderByMemExp!, new[] { sourceParam });

            MethodInfo orderByMethod = sortExpression.DirectionIndicator == SortOrderDirectionEnum.descending ?
                                            OrderByDescMethod.MakeGenericMethod(new[] { typeof(T), orderByMemExp!.Type }) :
                                            OrderByMethod.MakeGenericMethod(new[] { typeof(T), orderByMemExp!.Type });

            //Call OrderBy or OrderByDescending on the source IQueryable<T>
            return (IQueryable<T>)orderByMethod.Invoke(null, new object[] { source, orderByLamda })!;
        }

        // dynamically builds a ThenBy statement and adds it to the IQueryable{T} before returning that IQueryable{T}
        private IQueryable<T> ThenByDynamic(IQueryable<T> source, IQuerySortOrder<T> sortExpression)
        {
            //We need to convert the key selector from Expression<Func<T, object>> to a strongly typed Expression<Func<T, TKey>>
            //in order for Entity Framework to be able to translate it to SQL
            Expression? orderByMemExp = RemoveConvert(sortExpression.OrderedProperty.Body) as MemberExpression;
            ParameterExpression sourceParam = sortExpression.OrderedProperty.Parameters[0];

            LambdaExpression orderByLamda = Expression.Lambda(orderByMemExp!, new[] { sourceParam });

            MethodInfo orderByMethod = sortExpression.DirectionIndicator == SortOrderDirectionEnum.descending ?
                                            ThenByDescMethod.MakeGenericMethod(new[] { typeof(T), orderByMemExp!.Type }) :
                                            ThenByMethod.MakeGenericMethod(new[] { typeof(T), orderByMemExp!.Type });

            //Call OrderBy or OrderByDescending on the source IQueryable<T>
            return (IQueryable<T>)orderByMethod.Invoke(null, new object[] { source, orderByLamda })!;
        }
        #endregion

        #region RESEARCH REQUIRED - finish researching and coding this dynamic grouping feature. For now, individual repositories can implement their own methods if grouping is required
        // TODO: finish implementing the grouping logic
        private IQueryable<IGrouping<object, T>> AddGroupingToQuery(IQueryable<T> source, ISpecification<T> spec)
        {
            IQueryable < IGrouping<object, T>> groupingQuery = source.GroupBy(spec.GroupBy);
            //groupingQuery = groupingQuery.Select(spec.GroupingProjection);

            return groupingQuery;
        } 
        #endregion

        /// <summary>
        /// Applies paging to an existing IQueryable object
        /// </summary>
        /// <param name="query">an IQueryable object</param>
        /// <param name="page">the page of results to be requested</param>
        /// <param name="pageSize">the size of the page</param>
        /// <returns></returns>
        private IQueryable<T> AddPagingToQuery(IQueryable<T> query, int page = 0, int pageSize = 0)
        {
            if (page == 0 && pageSize == 0)
            {
                return query;
            }
            else
            {
                IQueryable<T> pagedQuery = query.Skip(page*pageSize).Take(pageSize);
                
                return pagedQuery;
            }
        }

        /// <summary>
        /// Gets all of the entities of type T
        /// </summary>
        /// <returns>a List of DomainObjects of type T</returns>
        public virtual List<T> GetAll()
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            return _dbContext.Set<T>().ToList();
        }

        public virtual List<T> GetAllWithSpecification(ISpecification<T> spec)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrdersToQuery(query, spec.SortOrderList);

            query = ExtendQueryWithOnlyIncludes(spec, query);
            return query.ToList();
        }

        public virtual BaseModelPagedList<T> GetAllWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrdersToQuery(query, spec.SortOrderList);

            query = ExtendQueryWithOnlyIncludes(spec, query);
            // only add the Skip and Take if both paging elements were provided
            if (!(page == 0 && pageSize == 0))
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            BaseModelPagedList<T> depl = new BaseModelPagedList<T>();
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            depl.EntityList = query.ToList();
            return depl;
        }

        public virtual BaseModelPagedList<T> GetAllWithSortOrders(List<SpecificationSortOrder<T>> sortOrders, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            BaseModelPagedList<T> depl = new BaseModelPagedList<T>();
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrdersToQuery(query, sortOrders);
            // only add the Skip and Take if both paging elements were provided
            if (!(page == 0 && pageSize == 0))
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            depl.EntityList = query.ToList();
            return depl;
        }

        public virtual BaseModelPagedList<T> GetAllWithSortOrder(SpecificationSortOrder<T> sortOrder, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            BaseModelPagedList<T> depl = new BaseModelPagedList<T>();
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrderToQuery(query, sortOrder);
            // only add the Skip and Take if both paging elements were provided
            if (!(page == 0 && pageSize == 0))
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            depl.EntityList = query.ToList();
            return depl;
        }

        public virtual T GetFirst()
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            return _dbContext.Set<T>().First();
        }

        /// <summary>
        /// this method will build the query using a combination of the Includes and NavigationPropertyIncludes, depending upon their existence, and return the first result.
        /// </summary>
        /// <param name="spec">an <seealso cref="ISpecification{T}"/>ISpecification object</param>
        /// <returns>a single item of type T that matches the ISpecification passed in</returns>
        public virtual T? GetItemWithSpecification(ISpecification<T> spec)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = ExtendQueryWithAllIncludes(spec, query);

            T? itemToReturn;
            try
            {
                itemToReturn = query.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Logger?.LogError(ex, "An exception occured in GetItemWithSpecification...possibly null data in a property marked as not nullable in the model.");
                itemToReturn = null;
            }
            return itemToReturn;
        }

        /// <summary>
        /// this method will build the query using a combination of the Includes and NavigationPropertyIncludes, depending upon their existence, and return the result count
        /// </summary>
        /// <param name="spec">an <seealso cref="ISpecification{T}"/>ISpecification object</param>
        /// <returns>the count of items of type T matching the ISpecification object passed in</returns>        
        public virtual int GetItemCountWithSpecification(ISpecification<T> spec)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = ExtendQueryWithAllIncludes(spec, query);

            return query.Count();
        }

        private int GetItemCount()
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            return query.Count();
        }

        public virtual IProjectedModel? GetProjectedItemWithSpecification(IProjection projection, ISpecification<T> spec)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

            IQueryable<IProjectedModel>? projectedQuery = null;
            projectedQuery = ExtendProjectedQueryWithAllIncludes(projection, spec, query);

            return projectedQuery.FirstOrDefault();
        }

        /// <summary>
        /// this method will build the query using a combination of the Includes and NavigationPropertyIncludes, depending upon their existence, and return a list of items. If paging data 
        /// was provided, it will be a paged set.
        /// </summary>
        /// <param name="spec">an <seealso cref="ISpecification{T}"/>ISpecification object</param>
        /// <param name="page">if paged results are desired, pass in the page number to return in this parameter</param>
        /// <param name="pageSize">if paged results are desired, pass in the page size in this parameter</param>
        /// <returns>a List of items of type T that match the ISpecification{T} passed in. If page and pageSize were supplied, this will be a subset of the total matches</returns>
        public virtual BaseModelPagedList<T> GetListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            BaseModelPagedList<T> depl = new BaseModelPagedList<T>();

            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrdersToQuery(query, spec.SortOrderList);

            depl.EntityPopulationCount = GetItemCountWithSpecification(spec);

            query = ExtendQueryWithAllIncludes(spec, query);

            // only add the Skip and Take if both paging elements were provided
            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            try
            {
                depl.EntityList = query.ToList();
                if(depl.EntityList != null && depl.Count == 1)
                {
                    // On occasion, and I think it is only when there are left joins involved, EF decides that there is one item 
                    // in the EntityList. However, the first item is null. Adding this code to remove that phantom record. This could 
                    // be the cause of some very hard to diagnose bugs.
                    if(depl.EntityList.FirstOrDefault() == null)
                    {
                        depl.EntityList.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An exception occured in GetListWithSpecification...possibly null data in a property marked as not nullable in the model.");
                depl.EntityList = null;
            }
            return depl;
        }

        public virtual List<IGrouping<object,T>> GetGroupedListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable(); 
            query = AddSortOrdersToQuery(query, spec.SortOrderList);
            query = ExtendQueryWithAllIncludes(spec, query);

            // only add the Skip and Take if both paging elements were provided
            if (!(page == 0 && pageSize == 0))
            {
                query = query.Skip(page * pageSize).Take(pageSize);
            }

            IQueryable<IGrouping<object, T>> groupQuery = AddGroupingToQuery(query, spec);

            int count = groupQuery.Count();

            return groupQuery.ToList();
        }

        public virtual BaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>>? sortOrderList = null, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            BaseModelPagedList<T> depl = new BaseModelPagedList<T>();
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (sortOrderList != null)
            {
                query = AddSortOrdersToQuery(query, sortOrderList); 
            }

            depl.EntityPopulationCount = GetItemCount();

            // only add the Skip and Take if both paging elements were provided
            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            try
            {
                depl.EntityList = query.ToList();
                if (depl.EntityList != null && depl.Count == 1)
                {
                    // On occasion, and I think it is only when there are left joins involved, EF decides that there is one item 
                    // in the EntityList. However, the first item is null. Adding this code to remove that phantom record. This could 
                    // be the cause of some very hard to diagnose bugs.
                    if (depl.EntityList.FirstOrDefault() == null)
                    {
                        depl.EntityList.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An exception occured in GetPagedList.");
                depl.EntityList = null;
            }

            return depl;
        }

        public virtual ProjectedModelPagedList GetProjectedListWithSpecification(IProjection projection, ISpecification<T> spec, int page = 0, int pageSize = 0)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            ProjectedModelPagedList depl = new ProjectedModelPagedList();

            IQueryable<T>? query = null;
            try
            {
                query = _dbContext.Set<T>().AsQueryable();
            }
            catch(Exception ex)
            {
                Logger?.LogError(ex, "Error in BaseEFRepository.GetProjectedListWithSpecification");
                query = _dbContext.Set<T>(typeof(T).ToString()).AsQueryable();
            }
            query = AddSortOrdersToQuery(query, spec.SortOrderList);

            IQueryable<IProjectedModel>? projectedQuery = null;
            projectedQuery = ExtendProjectedQueryWithAllIncludes(projection, spec, query);

            // only add the Skip and Take if both paging elements were provided
            if (!(page == 0 && pageSize == 0))
            {
                projectedQuery = projectedQuery.Skip((page - 1) * pageSize).Take(pageSize);
            }
            depl.ModelPopulationCount = GetItemCountWithSpecification(spec);
            depl.PageNumber = page;
            depl.PageSize = pageSize;
            depl.ModelList = projectedQuery.ToList();
            return depl;
        }

        public virtual ProjectedModelList GetUnpagedProjectedListWithSpecification(IProjection projection, ISpecification<T> spec)
        {
            ProjectedModelList depl = new ProjectedModelList();

            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = AddSortOrdersToQuery(query, spec.SortOrderList);

            IQueryable<IProjectedModel>? projectedQuery = null;
            projectedQuery = ExtendProjectedQueryWithAllIncludes(projection, spec, query);

            depl.ModelList = projectedQuery.ToList();
            return depl;
        }

        public virtual async Task<int> ExecuteStoredProcedure(string spName, params QueryParameter[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(spName).Append(" ");

            SqlParameter[]? parameters = null;
            if (args != null && args.Length != 0)
            {
                parameters = new SqlParameter[args.Length];
            }

            if (args?.Length > 0)
            {
                int pos = 0;
                foreach (QueryParameter arg in args)
                {
                    if (pos != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(arg.ParamName);
                    SqlParameter parameter = new SqlParameter()
                    {
                        ParameterName = arg.ParamName,
                        SqlDbType = MapParamTypeToDbType(arg.ParamType),
                        Size = arg.ParamLength,
                        Direction = System.Data.ParameterDirection.Input,
                        Value = arg.ParamValue
                    };
                    parameters![pos] = parameter;

                    pos++;
                }
            }

            string spStmt = sb.ToString();
            int rowsAffected = 0;
            if (parameters == null)
            {
                try
                {
                    rowsAffected = await ((DbContext)_dbContext).Database.ExecuteSqlRawAsync(sb.ToString());
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Exception in ExecuteStoredProcedure {spName}");
                    rowsAffected = -1;
                }
            }
            else
            {
                try
                {
                    rowsAffected = await ((DbContext)_dbContext).Database.ExecuteSqlRawAsync(spStmt, parameters);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Exception in ExecuteStoredProcedure {spName}");
                    rowsAffected = -1;
                }
            }

            return rowsAffected;
        }

        private SqlDbType MapParamTypeToDbType(QueryParameterTypeEnum qpType)
        {
            SqlDbType dbType = SqlDbType.VarChar;

            switch (qpType)
            {
                case QueryParameterTypeEnum.number:
                    {
                        dbType = SqlDbType.Int;
                        break;
                    }
                case QueryParameterTypeEnum.varchar:
                    {
                        dbType = SqlDbType.VarChar;
                        break;
                    }
            }

            return dbType;
        }

        public virtual List<T>? GetListFromSql(string sql, params QueryParameter[] args)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_GET);
            List<T>? resultList = null;
            if (args?.Length > 0)
            {
                int pos = 0;
                foreach(QueryParameter arg in args)
                {
                    if(pos != 0)
                    {
                        sql += ",";
                    }
                    if(arg.ParamType == QueryParameterTypeEnum.varchar || arg.ParamType == QueryParameterTypeEnum.date)
                    {
                        sql += String.Format(" {0} = \"{1}\"", arg.ParamName, arg.ParamValue);
                    }
                    else
                    {
                        sql += String.Format(" {0} = {1}", arg.ParamName, arg.ParamValue); 
                    }
                    pos++;
                }
            }

            resultList = _dbContext.Set<T>().FromSqlRaw(sql).ToList();

            return resultList;
        }

        public virtual T? GetItemFromSql(string sql, params QueryParameter[] args)
        {
            T? model = null;
            if (args?.Length > 0)
            {
                int pos = 0;
                foreach (QueryParameter arg in args)
                {
                    if (pos != 0)
                    {
                        sql += ",";
                    }
                    if (arg.ParamType == QueryParameterTypeEnum.varchar || arg.ParamType == QueryParameterTypeEnum.date)
                    {
                        sql += String.Format(" {0} = \"{1}\"", arg.ParamName, arg.ParamValue);
                    }
                    else
                    {
                        sql += String.Format(" {0} = {1}", arg.ParamName, arg.ParamValue);
                    }
                    pos++;
                }
            }

            model = (T?)_dbContext.Set<T>().FromSqlRaw(sql).AsEnumerable().Take(1).FirstOrDefault();

            return model;
        }

        private T PreActionValidation(T entity)
        {
            if(Validator == null)
            {
                return entity;// if no validator, assume true
            }

            var validationResults = Validator.Validate(entity);
            if (!validationResults.IsValid)
            {
                entity.IsValid = false;
                entity.ValidationErrors = validationResults.ValidationErrors!;
            }
            return entity;
        }

        /// <summary>
        /// Adds an entity of type T to the DbContext
        /// </summary>
        /// <param name="entity">a BaseModel of type T</param>
        /// <returns>the added entity. NOTE: the <seealso cref="ISupportsValidation"/> properties are used to pass any error information back in the entity.</returns>
        public virtual T Add(T entity)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_ADD);
            DatabaseActionResult? result = null;
            entity = PreActionValidation(entity);
            if (!entity.IsValid) {
                return entity;
            }
            
            try
            {
                EntityEntry newEntity = _dbContext.Set<T>().Add(entity);
                // if we are not in a Transaction (the UnitOfWork will take care of triggering the save when the Transaction is complete) and additions were made, save said additions.
                if (!InTransaction && newEntity?.State == EntityState.Added)
                {
                    result = SaveChanges() as DatabaseActionResult;
                    entity.IsValid = result!.Succeeded;
                }
            }
            catch (Exception ex)
            {
                entity.IsValid = false;
                entity.ValidationErrors.Add("SaveError", "A critical error was encountered while attempting to save your data.");
                if (!InTransaction)
                {
                    Logger?.LogError(ex, "Error in BaseEFRepository.Add"); 
                }
            }
            return entity;
        }

        /// <summary>
        /// Adds a collection of DomainObjects of type T to the DbContext
        /// </summary>
        /// <param name="models">a List of DomainObjects of type T</param>
        /// <returns>true if added, false if not. NOTE: the <seealso cref="ISupportsValidation"/> properties are used to pass any error information back in the entity.</returns>
        public virtual bool AddRange(List<T> models)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_ADD);
            bool successful = true;

            foreach (T model in models)
            {
                var rModel = PreActionValidation(model);
                if (!rModel.IsValid)
                {
                    model.IsValid = false;
                    model.ValidationErrors = rModel.ValidationErrors;
                    successful = false; 
                    break;
                }
            }

            if(!successful)
            {
                return successful;
            }

            try
            {
                _dbContext.Set<T>().AddRange(models.ToArray());
                // if we are not in a Transaction (the UnitOfWork will take care of triggering the save when the Transaction is complete) and additions were made, save said additions.
                if (!InTransaction)
                {
                    var result = SaveChanges() as DatabaseActionResult;
                    successful = result!.Succeeded;
                }
            }
            catch (Exception ex)
            {
                // TODO: log the exception
                Logger?.LogError(ex, "Error in BaseEFRepository.AddRange");
                successful = false;
            }

            return successful;
        }

        /// <summary>
        /// Updates the entity in the DbContext that matches the entity provided as an argument. There is a section for updates that will process collections. It is important to note that the IAuditableObject interface properties 
        /// for those objects will be set here and not in SaveChanges. SaveChanges will set the values for the parent object only.
        /// </summary>
        /// <param name="entityIn">a BaseModel of type T</param>
        /// <exception cref="EntityNotFoundInRepositoryException">This is thrown if no matching entity is found in the Context</exception>
        /// <returns>the updated entity. NOTE: the <seealso cref="ISupportsValidation"/> properties are used to pass any error information back in the entity.</returns>
        public virtual T Update(T entityIn)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_UPDATE);
            entityIn = PreActionValidation(entityIn);
            if (!entityIn.IsValid)
            {
                return entityIn;
            }

            T? entityInRepository;
            try
            {
                int entityPrimaryKeyValue = GetKeyValueFromEntity(entityIn);
                entityInRepository = _dbContext.Set<T>().Find(entityPrimaryKeyValue);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error in BaseEFRepository.Update");
                Guid guidPrimaryKeyValue = GetGuidKeyValueFromEntity(entityIn);
                entityInRepository = _dbContext.Set<T>().Find(guidPrimaryKeyValue);

            }

            if (entityInRepository == null)
            {
                var ex = new EntityNotFoundInRepositoryException(typeof(T).ToString());
                Logger?.LogError(ex, "Error in BaseEFRepository.Update");
                throw ex;
            }
            else
            {
                try
                {
                    EntityEntry<T> dbEntry = _dbContext.Entry(entityInRepository);
                    string entityPrimaryKeyName = _dbContext.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.Select(x => x.Name).Single();

                    // get all of the object's properties
                    PropertyInfo[] sourceEntityProperties = entityIn.GetType().GetProperties();

                    // loop through each of the properties so we can set its analogous property in the attached entity in the repository
                    foreach (PropertyInfo property in sourceEntityProperties)
                    {
                        if (null != property.GetSetMethod())
                        {
#nullable enable
                            PropertyInfo? propInTargetEntity = entityInRepository.GetType().GetProperty(property.Name);
#nullable disable

                            // Once added, the Primary Key, Created, or CreatedBy members of the entity should be immutable.
                            if (propInTargetEntity.Name != entityPrimaryKeyName && propInTargetEntity.Name != Constants.AUDIT_COLUMNS_CREATED_DATE && propInTargetEntity.Name != Constants.AUDIT_COLUMNS_CREATED_BY_USER)
                            {
                                // Collection properties are handled very differently. The convention I have adopted is that if the collection is NULL, that indicates the collection is 
                                // excluded from updates. If it has been hydrated, process the entries in the target object
                                if (propInTargetEntity.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propInTargetEntity.PropertyType.GetGenericTypeDefinition()))
                                {
                                    // get the collection (the current property is a collection according to the "if" conditions)
                                    object collObject = property.GetValue(entityIn, null);
                                    if (collObject != null)
                                    {
                                        IEnumerable coAsEnum = collObject as IEnumerable;

                                        // the collection (in the disconnected entity object) is not null, so process its contents
                                        string propertyName = property.Name;

                                        // get the analogous collection from the repository object, create an accessor, and load the repository objects into the collection
                                        CollectionEntry dbItemsEntry = dbEntry.Collection(propertyName);
                                        IClrCollectionAccessor accessor = dbItemsEntry.Metadata.GetCollectionAccessor();
                                        dbItemsEntry.Load();

                                        Dictionary<int, BaseModel> dbItemsMap = new Dictionary<int, BaseModel>();
                                        // map the contents of the connected object's collection using the Id
                                        foreach (var element in dbItemsEntry.CurrentValue)
                                        {
                                            int pk = GetKeyValueFromEntityForCollectionProcessing(element);
                                            dbItemsMap.Add(pk, (BaseModel)element);
                                        }

                                        // get the elements from the disconnected entity's collection
                                        IEnumerable<BaseModel> items = (IEnumerable<BaseModel>)accessor.GetOrCreate(entityIn, true);

                                        foreach (BaseModel item in items)
                                        {
                                            int itemKeyValue = GetKeyValueFromEntityForCollectionProcessing(item);

                                            // add to the repository object's collection if not found
                                            BaseModel oldItem;
                                            if (!dbItemsMap.TryGetValue(itemKeyValue, out oldItem))
                                            {
                                                accessor.Add(dbEntry.Entity, item, true);
                                                // remove the item from the map. this will allow us to determine which ones need to be deleted below
                                                dbItemsMap.Remove(itemKeyValue);
                                            }
                                            else
                                            {
                                                // update the object if found
                                                _dbContext.Entry(oldItem).CurrentValues.SetValues(item);

                                                // remove the item from the map. this will allow us to determine which ones need to be deleted below
                                                dbItemsMap.Remove(itemKeyValue);
                                            }
                                        }

                                        // perform the necessary deletes
                                        foreach (BaseModel oldItem in dbItemsMap.Values)
                                        {
                                            accessor.Remove(dbEntry.Entity, oldItem);
                                        }
                                    }
                                }
                                else
                                {
                                    // if we are not dealing with a "not mapped" property, set the value
                                    if (!propInTargetEntity.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
                                    {
                                        propInTargetEntity.SetValue(entityInRepository, property.GetValue(entityIn, null), null);
                                    }
                                }
                            }
                        }
                    }

                    // if we are not in a Transaction (the UnitOfWork will take care of triggering the save when the Transaction is complete) and changes were made, save said changes.
                    if (!InTransaction && dbEntry?.State == EntityState.Modified)
                    {
                        var result = SaveChanges();
                        entityInRepository.IsValid = result.Succeeded;
                    }
                }
                catch (Exception ex)
                {
                    entityInRepository.IsValid = false;
                    if (!InTransaction)
                    {
                        Logger?.LogError(ex, "Error in BaseEFRepository.Update"); 
                    }
                }
            }

            return entityInRepository;
        }

        /// <summary>
        /// Updates a collection of entities in the DbContext
        /// </summary>
        /// <param name="models">a List of DomainObjects of Type T</param>
        /// <returns>true if updates successful, false if not.</returns>
        public virtual bool UpdateRange(List<T> models)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_UPDATE);
            bool successful = true;

            foreach (T model in models)
            {
                var rModel = PreActionValidation(model);
                if (!rModel.IsValid)
                {
                    model.IsValid = false;
                    model.ValidationErrors = rModel.ValidationErrors;
                    successful = false;
                    break;
                }
            }

            if (!successful)
            {
                return successful;
            }

            try
            {
                _dbContext.Set<T>().UpdateRange(models.ToArray());
                // if we are not in a Transaction (the UnitOfWork will take care of triggering the save when the Transaction is complete) and changes were made, save said changes.
                if (!InTransaction)
                {
                    var result = SaveChanges();
                    successful = result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                // log the exception
                Logger?.LogError(ex, "Error in BaseEFRepository.UpdateRange");
                successful = false;
            }

            return successful;
        }

        /// <summary>
        /// Deletes from the DbContext a BaseModel of type T
        /// </summary>
        /// <param name="entity">a BaseModel of type T</param>
        /// <returns>true if deleted, false if not.</returns>
        public virtual bool Delete(T entity)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_DELETE);
            try
            {
                _dbContext.Set<T>().Remove(entity);
                if (!InTransaction)
                {
                    var result = SaveChanges();
                    return result.Succeeded;
                }
                return true;
            }
            catch (Exception ex)
            {
                // log this
                if (!InTransaction)
                {
                    Logger?.LogError(ex, "Error in BaseEFRepository.Delete"); 
                }
                return false;
            }
        }

        /// <summary>
        /// Deletes from the DbContext a collection of DomainObjects
        /// </summary>
        /// <param name="models">a list of DomainObjects of type T</param>
        /// <returns>true if delete, false if not.</returns>
        public virtual bool DeleteRange(List<T> models)
        {
            Validator?.SetDatabaseActionContext(Constants.DATABASE_ACTION_DELETE);
            bool successful = true;
            try
            {
                _dbContext.Set<T>().RemoveRange(models.ToArray());
                if (!InTransaction)
                {
                    var result = SaveChanges();
                    return result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                // TODO: log the exception
                Logger?.LogError(ex, "Error in BaseEFRepository.DeleteRange");
                successful = false;
            }

            return successful;
        }

        public virtual void DetachEntity(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Called to obtain the key value from the entity provided
        /// </summary>
        /// <param name="entity">a BaseModel of type T</param>
        /// <returns>an integer value that is the primary key for the object passed as an argument IF the table has a primary key that is type int</returns>
        /// <exception cref="InvalidCastException">Throws an InvalidCastException if the table's primary key is not an int</exception>
        protected int GetKeyValueFromEntity(T entity)
        {
            string keyName = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        /// <summary>
        /// Called to obtain the key value from the entity provided
        /// </summary>
        /// <param name="entity">a BaseModel of type T</param>
        /// <returns>an integer value that is the primary key for the object passed as an argument IF the table has a primary key that is type Guid</returns>
        /// <exception cref="InvalidCastException">Throws an InvalidCastException if the table's primary key is not a Guid</exception>
        protected Guid GetGuidKeyValueFromEntity(T entity)
        {
            string keyName = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            return (Guid)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        protected int GetKeyValueFromEntityForCollectionProcessing(object entity)
        {
            Type t = entity.GetType();
            string keyName = _dbContext.Model.FindEntityType(t).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if(disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This function performs the saving of the data to the underlying data store
        /// </summary>
        /// <param name="inTransaction"></param>
        /// <returns>IDatabaseActionResult</returns>
        /// <remarks>NOTE: this function should never be called from the Add, Updated, or Delete functions IF the repository is part of a Transaction</remarks>
        public virtual IDatabaseActionResult SaveChanges()
        {
            DatabaseActionResult result = new() { Succeeded = true };
            try
            {
                RepositoryPreSaveEventArgs<T> args = null;
                IEnumerable<EntityEntry> changes = _dbContext.ChangeTracker.Entries();
                foreach (EntityEntry entityEntry in changes)
                {
                    args = SignalPreSaveEventHandlers(entityEntry as T);
                    if (args.CancelSave)
                    {
                        result.Succeeded = false;
                    } 
                }

                if (result.Succeeded)
                {
                    result = _dbContext.Save() as DatabaseActionResult;
                    SignalSaveEventHandlers(result.Succeeded);
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error in BaseEFRepository.SaveChanges");
                result.Exception = ex;
            }

            return result;
        }

        public RepositoryPreSaveEventArgs<T> SignalPreSaveEventHandlers(T model)
        {
            RepositoryPreSaveEventArgs<T> e = new RepositoryPreSaveEventArgs<T>(model);
            OnRaisePreSaveEvent(e);
            return e;
        }

        public void SignalSaveEventHandlers(bool saveSuccessful)
        {
            RepositorySaveEventArgs e = new RepositorySaveEventArgs(saveSuccessful);
            OnRaiseSaveEvent(e);
        }

        public bool HasChanges()
        {
            bool hasChanges = _dbContext.ChangeTracker.HasChanges();

            return hasChanges;
        }

        public string ModelType()
        {
            return _dbContext.ChangeTracker.Entries().FirstOrDefault()?.Entity.GetType().Name;
        }
    }
}
