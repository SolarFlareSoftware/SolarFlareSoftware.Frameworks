//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Specifications;
using System;
using System.Collections.Generic;
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
            T? objT = GetById(id);
            if (objT == null) return false;

            return Repository.Delete(objT);
        }

        public virtual bool DeleteById(int id)
        {
            T? objT = GetById(id);
            if (objT == null) return false;

            return Repository.Delete(objT);
        }

        public virtual ICollection<T> GetAll()
        {
            return Repository.GetAll();
        }

        public abstract T? GetById(Guid id);

        public abstract T? GetById(int id);

        public virtual T? GetBySpecification(ISpecification<T> spec)
        {
            return Repository.GetItemWithSpecification(spec);
        }

        public virtual IBaseModelPagedList<T> GetPagedList(ISpecification<T> spec, int page = 1, int pageSize = 10)
        {
            return Repository.GetListWithSpecification(spec, page, pageSize);
        }

        public virtual T Update(T entity)
        {
            return Repository.Update(entity);
        }

        public virtual IBaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>>? sortOrders = null, int page = 1, int pageSize = 10)
        {
            return Repository.GetPagedList(sortOrders, page, pageSize);
        }
    }
}
