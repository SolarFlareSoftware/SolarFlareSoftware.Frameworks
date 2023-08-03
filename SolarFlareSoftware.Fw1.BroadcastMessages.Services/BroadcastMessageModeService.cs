using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using SolarFlareSoftware.Fw1.Core.ServiceInterfaces;
using SolarFlareSoftware.Fw1.Core.Specifications;
using SolarFlareSoftware.Fw1.Services.Core;
using System;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.BroadcastMessages.Services
{
    public class BroadcastMessageModeService : BaseService<BroadcastMessageMode>, IBroadcastMessageModeService
    {
        public IValidationResult ValidationResultDictionaryWrapper { get; set; }
        public BroadcastMessageModeService(IUnitOfWork unitOfWork, IRepository<BroadcastMessageMode> repo, IPrincipal principal, IValidationResult validationResultDictionary)
            :base(unitOfWork, repo, principal)
        {
            ValidationResultDictionaryWrapper = validationResultDictionary;
        }
        public BaseModelPagedList<BroadcastMessageMode> GetActiveBroadcastMessageModes()
        {
            return Repository.GetListWithSpecification(new BaseSpecification<BroadcastMessageMode>(x => x.IsActive));
        }

        public override BroadcastMessageMode GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override BroadcastMessageMode GetById(int id)
        {
            return Repository.GetItemWithSpecification(new BaseSpecification<BroadcastMessageMode>(x=>x.BroadcastMessageModeID == id));
        }
    }
}
