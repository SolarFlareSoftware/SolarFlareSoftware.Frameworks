using SolarFlareSoftware.Fw1.Core.Events;
using SolarFlareSoftware.Fw1.Core.Models;
using SolarFlareSoftware.Fw1.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IRepository<T> : IBaseModelRepository<T> where T : IBaseModel 
    {
        public event EventHandler<RepositoryPreSaveEventArgs<T>> RepositoryPreSaveEvent;
        public event EventHandler<RepositorySaveEventArgs> RepositorySaveEvent;

        T GetItemWithSpecification(ISpecification<T> spec);

        int GetItemCountWithSpecification(ISpecification<T> spec);

        IProjectedModel GetProjectedItemWithSpecification(IProjection projection, ISpecification<T> spec);

        BaseModelPagedList<T> GetListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        List<IGrouping<object, T>> GetGroupedListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        ProjectedModelPagedList GetProjectedListWithSpecification(IProjection projection, ISpecification<T> spec, int page = 0, int pageSize = 0);
        ProjectedModelList GetUnpagedProjectedListWithSpecification(IProjection projection, ISpecification<T> spec);

        List<T> GetAll();
        List<T> GetAllWithSpecification(ISpecification<T> spec);
        BaseModelPagedList<T> GetAllWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetAllWithSortOrders(List<SpecificationSortOrder<T>> sortOrders, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetAllWithSortOrder(SpecificationSortOrder<T> sortOrder, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>> sortOrders = null, int page = 0, int pageSize = 0);

        List<T> GetListFromSql(string sql, params QueryParameter[] args);

        T GetItemFromSql(string sql, params QueryParameter[] args);

        Task<int> ExecuteStoredProcedure(string spName, params QueryParameter[] args);

        T Add(T model);

        bool AddRange(List<T> models);

        T Update(T model);

        bool UpdateRange(List<T> models);

        bool Delete(T model);

        bool DeleteRange(List<T> models);

        void DetachEntity(T entity);
    }
}
