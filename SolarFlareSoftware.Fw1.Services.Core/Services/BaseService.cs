using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Specifications;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.Services.Core
{
    public abstract class BaseService<T> : IService<T> where T : IBaseModel
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IRepository<T> Repository { get; set; }
        public IPrincipal User { get; set; }

        public BaseService(IUnitOfWork unitOfWork, IRepository<T> repository, IPrincipal user)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            User = user;
        }

        public virtual T Add(T entity)
        {
            return Repository.Add(entity);
        }

        public virtual bool Delete(T entity)
        {
            return Repository.Delete(entity);
        }

        public virtual bool DeleteById(Guid id)
        {
            return Repository.Delete(GetById(id));
        }

        public virtual bool DeleteById(int id)
        {
            return Repository.Delete(GetById(id));
        }

        public virtual ICollection<T> GetAll()
        {
            return Repository.GetAll();
        }

        public abstract T GetById(Guid id);

        public abstract T GetById(int id);

        public virtual T GetBySpecification(ISpecification<T> spec)
        {
            return Repository.GetItemWithSpecification(spec);
        }

        public virtual IBaseModelPagedList<T> GetPagedList(ISpecification<T> spec, int page, int pageSize)
        {
            return Repository.GetListWithSpecification(spec, page, pageSize);
        }

        public virtual T Update(T entity)
        {
            return Repository.Update(entity);
        }

        public IBaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>> sortOrders = null, int page = 0, int pageSize = 0)
        {
            return Repository.GetPagedList(sortOrders, page, pageSize);
        }
    }
}
