using SolarFlareSoftware.Fw1.Core.Specifications;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IService<T> where T : IBaseModel
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IRepository<T> Repository { get; set; }
        public IPrincipal User { get; set; }
        public T Add(T entity);
        public bool Delete (T entity);
        public bool DeleteById(Guid id);
        public bool DeleteById(int id);
        public abstract T GetById(Guid id);
        public abstract T GetById(int id);
        public T GetBySpecification(ISpecification<T> spec);
        public IBaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>> sortOrders = null, int page=1, int pageSize=25);
        public IBaseModelPagedList<T> GetPagedList(ISpecification<T> spec, int page, int pageSize);
        public ICollection<T> GetAll();
        public T Update(T entity);
    }
}
