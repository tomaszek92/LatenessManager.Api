using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;
using LatenessManager.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LatenessManager.Application.Players.EventHandlers
{
    public class PlayerPenaltyCarriedOutEventHandler : INotificationHandler<DomainEventNotification<PlayerPenaltyCarriedOutEvent>>
    {
        private readonly ILogger<PlayerPenaltyCarriedOutEventHandler> _logger;

        public PlayerPenaltyCarriedOutEventHandler(ILogger<PlayerPenaltyCarriedOutEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<PlayerPenaltyCarriedOutEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {GetType().Name}, PlayerId: {notification.DomainEvent.PlayerId}");
            
            return Task.CompletedTask;
        }
    }
}