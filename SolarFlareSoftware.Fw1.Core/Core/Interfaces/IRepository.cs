/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
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

        T? GetItemWithSpecification(ISpecification<T> spec);

        int GetItemCountWithSpecification(ISpecification<T> spec);

        IProjectedModel? GetProjectedItemWithSpecification(IProjection projection, ISpecification<T> spec);

        BaseModelPagedList<T> GetListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        List<IGrouping<object, T>> GetGroupedListWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        ProjectedModelPagedList GetProjectedListWithSpecification(IProjection projection, ISpecification<T> spec, int page = 0, int pageSize = 0);
        ProjectedModelList GetUnpagedProjectedListWithSpecification(IProjection projection, ISpecification<T> spec);

        List<T> GetAll();
        List<T> GetAllWithSpecification(ISpecification<T> spec);
        BaseModelPagedList<T> GetAllWithSpecification(ISpecification<T> spec, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetAllWithSortOrders(List<SpecificationSortOrder<T>> sortOrders, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetAllWithSortOrder(SpecificationSortOrder<T> sortOrder, int page = 0, int pageSize = 0);

        BaseModelPagedList<T> GetPagedList(List<SpecificationSortOrder<T>>? sortOrders = null, int page = 0, int pageSize = 0);

        List<T>? GetListFromSql(string sql, params QueryParameter[] args);

        T? GetItemFromSql(string sql, params QueryParameter[] args);

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
