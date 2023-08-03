using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using SolarFlareSoftware.Fw1.Core.ServiceInterfaces;
using SolarFlareSoftware.Fw1.Core.Specifications;
using SolarFlareSoftware.Fw1.Services.Core;
using System;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.BroadcastMessages.Services
{
    public class BroadcastMessageTypeService : BaseService<BroadcastMessageType>, IBroadcastMessageTypeService
    {
        public IValidationResult ValidationResultDictionaryWrapper { get; set; }
        public BroadcastMessageTypeService(IUnitOfWork unitOfWork, IRepository<BroadcastMessageType> repo, IPrincipal principal, IValidationResult validationResultDictionary)
            : base(unitOfWork, repo, principal)
        {
            ValidationResultDictionaryWrapper = validationResultDictionary;
        }
        public BaseModelPagedList<BroadcastMessageType> GetActiveBroadcastMessageTypes()
        {
            return Repository.GetListWithSpecification(new BaseSpecification<BroadcastMessageType>(x => x.IsActive));
        }

        public override BroadcastMessageType GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override BroadcastMessageType GetById(int id)
        {
            return Repository.GetItemWithSpecification(new BaseSpecification<BroadcastMessageType>(x=>x.BroadcastMessageTypeID == id));
        }
    }
}
