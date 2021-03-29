using LatenessManager.Domain.Common;
using MediatR;

namespace LatenessManager.Application.Common.Models
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : BaseDomainEvent
    {
        public TDomainEvent DomainEvent { get; }
        
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}