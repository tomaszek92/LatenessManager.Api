using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;
using LatenessManager.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LatenessManager.Application.Players.EventHandlers
{
    public class PlayerPenaltyAddedEventHandler : INotificationHandler<DomainEventNotification<PlayerPenaltyAddedEvent>>
    {
        private readonly ILogger<PlayerPenaltyAddedEventHandler> _logger;

        public PlayerPenaltyAddedEventHandler(ILogger<PlayerPenaltyAddedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<PlayerPenaltyAddedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {GetType().Name}, PlayerId: {notification.DomainEvent.PlayerId}");

            return Task.CompletedTask;
        }
    }
}