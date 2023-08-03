using SolarFlareSoftware.Fw1.Core;
using SolarFlareSoftware.Fw1.Core.Events;
using SolarFlareSoftware.Fw1.Core.Interfaces;
using SolarFlareSoftware.Fw1.Core.Models;
using SolarFlareSoftware.Fw1.Core.ServiceInterfaces;
using SolarFlareSoftware.Fw1.Core.Specifications;
using SolarFlareSoftware.Fw1.Services.Core;
using System;
using System.Security.Principal;

namespace SolarFlareSoftware.Fw1.BroadcastMessages.Services
{
    public class BroadcastMessageService : BaseService<BroadcastMessage>, IBroadcastMessageService
    {
        public IRepository<BroadcastMessageHistory> HistoryRepository { get; set; }
        public IValidationResult ValidationResultDictionaryWrapper { get; set; }
        public IPrincipal Principal { get; set; }

        public BroadcastMessageService(IUnitOfWork unitOfWork, IRepository<BroadcastMessage> repo, IRepository<BroadcastMessageHistory> historyRepo, IValidationResult validationResultDictionary, IPrincipal principal)
            : base(unitOfWork, repo, principal)
        {
            HistoryRepository = historyRepo;
            ValidationResultDictionaryWrapper = validationResultDictionary;
            Principal = principal;
        }
        public BaseModelPagedList<BroadcastMessage> GetActiveBannerMessages()
        {
            return Repository.GetListWithSpecification(new BaseSpecification<BroadcastMessage>(x => x.IsActive && x.BroadcastMessageModeID == Constants.BROADCAST_MESSAGE_MODE_BANNER && x.BeginBroadcast <= DateTime.Now && (x.EndBroadcast == null || x.EndBroadcast >= DateTime.Now)));
        }
        public BaseModelPagedList<BroadcastMessage> GetActiveWidgetMessages()
        {
            return Repository.GetListWithSpecification(new BaseSpecification<BroadcastMessage>(x => x.IsActive && x.BroadcastMessageModeID == Constants.BROADCAST_MESSAGE_MODE_WIDGET && x.BeginBroadcast <= DateTime.Now && (x.EndBroadcast == null || x.EndBroadcast >= DateTime.Now)));
        }
        public BaseModelPagedList<BroadcastMessage> GetAllMessages( int page = 1, int pageSize = 20, string sortColumn = "AuditAddDate", SortOrderDirectionEnum sortDirection = SortOrderDirectionEnum.none)
        {
            BaseSpecification<BroadcastMessage> spec = new BaseSpecification<BroadcastMessage>(x => x.BroadcastMessageID > 0);
            spec.Includes.Add(x => x.BroadcastMessageMode);
            spec.Includes.Add(x => x.BroadcastMessageType);

            SpecificationSortOrder<BroadcastMessage> sortOrder = BuildSortDirectives(sortColumn, sortDirection);
            spec.SortOrderList.Add(sortOrder);

            BaseModelPagedList<BroadcastMessage> typesOut = Repository.GetListWithSpecification(spec, page, pageSize);
            
            return typesOut;
        }

        public override BroadcastMessage GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override BroadcastMessage GetById(int id)
        {
            return Repository.GetItemWithSpecification(new BaseSpecification<BroadcastMessage>(x=> x.BroadcastMessageID == id));
        }

        public BroadcastMessage SaveBroadcastMessage(BroadcastMessage broadcastMessage)
        {
            short action;
            BroadcastMessage savedMessage = null;
            UnitOfWork.BeginTransaction(Repository);
            UnitOfWork.JoinTransaction(HistoryRepository);
            if (broadcastMessage.BroadcastMessageID == 0)
            {
                action = Constants.HISTORY_TABLE_ACTION_INSERT;
                savedMessage = Repository.Add(broadcastMessage);
                GeneralSaveEventSubscriber<BroadcastMessage> sectionSaveEventSubscriber = new GeneralSaveEventSubscriber<BroadcastMessage>(Repository, savedMessage, action, Principal.Identity.Name);
                sectionSaveEventSubscriber.SaveNotificationEvent += BroadcastMessageSaveEventSubscriber_NotificationEvent;
            }
            else
            {
                BroadcastMessage existingMessage = GetById(broadcastMessage.BroadcastMessageID);
                action = Constants.HISTORY_TABLE_ACTION_UPDATE;
                existingMessage.BroadcastMessageModeID = broadcastMessage.BroadcastMessageModeID;
                existingMessage.BroadcastMessageTypeID = broadcastMessage.BroadcastMessageTypeID;
                existingMessage.MessageTitle = broadcastMessage.MessageTitle;
                existingMessage.MessageText = broadcastMessage.MessageText;
                existingMessage.BeginBroadcast = broadcastMessage.BeginBroadcast;
                existingMessage.EndBroadcast = broadcastMessage.EndBroadcast;
                existingMessage.IsActive = broadcastMessage.IsActive;
                savedMessage = existingMessage;
                GeneralSaveEventSubscriber<BroadcastMessage> sectionSaveEventSubscriber = new GeneralSaveEventSubscriber<BroadcastMessage>(Repository, savedMessage, action, Principal.Identity.Name);
                sectionSaveEventSubscriber.SaveNotificationEvent += BroadcastMessageSaveEventSubscriber_NotificationEvent;
            }
            bool saved = false;
            try
            {
                saved = UnitOfWork.Complete();
                if (saved)
                {
                    savedMessage.IsValid = true;
                }
                else
                {
                    UnitOfWork.Rollback();
                }
            }
            catch (Exception ex)
            {
                savedMessage.IsValid = false;
                savedMessage.ValidationErrors.Add("BroadcastMessageSaveError", "An error was encountered while updating. Please try again. Contact the Help Desk if this persists.");
            }
            return savedMessage;
        }

        public BroadcastMessage DeleteBroadcastMessage(int broadcastMessageID)
        {
            UnitOfWork.BeginTransaction(Repository);
            UnitOfWork.JoinTransaction(HistoryRepository);
            BroadcastMessage deletedMessage = GetById(broadcastMessageID);
            //BroadcastMessageHistory broadcastMessageHistory = deletedMessage.Adapt<BroadcastMessageHistory>();
            Repository.Delete(deletedMessage);
            //broadcastMessageHistory.ActionBy = Principal.Identity.Name;
            //broadcastMessageHistory.ActionCode = Constants.HISTORY_TABLE_ACTION_DELETE;
            //broadcastMessageHistory.ActionDate = DateTime.Now;
            //HistoryRepository.Add(broadcastMessageHistory);

            bool saved = false;
            try
            {
                saved = UnitOfWork.Complete();
                if (saved)
                {
                    deletedMessage.IsValid = true;
                }
                else
                {
                    UnitOfWork.Rollback();
                }
            }
            catch (Exception ex)
            {
                deletedMessage.IsValid = false;
                deletedMessage.ValidationErrors.Add("BroadcastMessageDeleteError", "An error was encountered while updating. Please try again. Contact the Help Desk if this persists.");
            }
            return deletedMessage;
        }

        private SpecificationSortOrder<BroadcastMessage> BuildSortDirectives(string sortColumn, SortOrderDirectionEnum sortDirection)
        {
            SpecificationSortOrder<BroadcastMessage> sortOrder = null;
            switch (sortColumn)
            {
                case "AuditAddDate":
                    {
                        sortOrder = new SpecificationSortOrder<BroadcastMessage>();
                        sortOrder.DirectionIndicator = sortDirection;
                        sortOrder.OrderedProperty = x => x.AuditAddDate;
                        break;
                    }
                case "IsActive":
                    {
                        sortOrder = new SpecificationSortOrder<BroadcastMessage>();
                        sortOrder.DirectionIndicator = sortDirection;
                        sortOrder.OrderedProperty = x => x.IsActive;
                        break;
                    }
                case "BroadcastMessageType":
                    {
                        sortOrder = new SpecificationSortOrder<BroadcastMessage>();
                        sortOrder.DirectionIndicator = sortDirection;
                        sortOrder.OrderedProperty = x => x.BroadcastMessageTypeID;
                        break;
                    }
                default:
                    {
                        sortOrder = new SpecificationSortOrder<BroadcastMessage>();
                        sortOrder.DirectionIndicator = SortOrderDirectionEnum.descending;
                        sortOrder.OrderedProperty = x => x.AuditAddDate;
                        break;
                    }
            }

            return sortOrder;
        }

        private void BroadcastMessageSaveEventSubscriber_NotificationEvent(object sender, ServiceSaveNotificationEventArgs e)
        {
            if (e.SaveSuccessful)
            {
                GeneralSaveEventSubscriber<BroadcastMessage> BroadcastMessageSaveEventSubscriber = (GeneralSaveEventSubscriber<BroadcastMessage>)sender;
                short action = BroadcastMessageSaveEventSubscriber.Action;
                BroadcastMessage BroadcastMessage = (BroadcastMessage)e.EntityBeingSaved;
                //BroadcastMessageHistory broadcastMessageHistory = BroadcastMessage.Adapt<BroadcastMessageHistory>();
                //broadcastMessageHistory.ActionBy = BroadcastMessageSaveEventSubscriber.ActionBy;
                //broadcastMessageHistory.ActionCode = action;
                //broadcastMessageHistory.ActionDate = DateTime.Now;
                //HistoryRepository.Add(broadcastMessageHistory);
            }
        }

    }
}
