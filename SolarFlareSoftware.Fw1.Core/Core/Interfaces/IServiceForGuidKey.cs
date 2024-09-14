//Copyright 2020-2024 Solar Flare Software, Inc. All Rights Reserved. Permission to use, copy, modify,
//and distribute this software and its documentation for educational, research, and not-for-profit purposes,
//without fee and without a signed licensing agreement is hereby prohibited. Contact Solar Flare Software, Inc.
//at 6834 Lincoln Way W, Saint Thomas, PA 17252 or at sales@solarflaresoftware.com for licensing opportunities.
using SolarFlareSoftware.Fw1.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IServiceForGuidKey<T> where T : IBaseModel
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IRepository<T> Repository { get; set; }
        public IPrincipal User { get; set; }
        public T Add(T entity);
        public bool Delete (T entity);
        public bool DeleteById(Guid id);
        public abstract T GetById(Guid id);
        public T GetBySpecification(ISpecification<T> spec);
        public IBaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>>? sortOrders = null, int page=1, int pageSize=10);
        public IBaseModelPagedList<T> GetPagedList(ISpecification<T> spec, int page = 1, int pageSize = 10);
        public ICollection<T> GetAll();
        public T Update(T entity);
    }
}
