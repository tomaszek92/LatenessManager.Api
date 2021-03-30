using System;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Common.Models;
using LatenessManager.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LatenessManager.Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IPublisher _publisher;

        public DomainEventService(ILogger<DomainEventService> logger, IPublisher publisher)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Publish(BaseDomainEvent baseDomainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Publishing domain event. Event - {baseDomainEvent.GetType().Name}");
            var notification = GetNotificationCorrespondingToDomainEvent(baseDomainEvent);
            await _publisher.Publish(notification, cancellationToken);
        }
        
        private static INotification GetNotificationCorrespondingToDomainEvent(BaseDomainEvent baseDomainEvent) => 
            (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(baseDomainEvent.GetType()), baseDomainEvent);
    }
}