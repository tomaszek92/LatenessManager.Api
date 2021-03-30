using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Common.Models;
using LatenessManager.Domain.Entities.PlayerAggregate;
using LatenessManager.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.EventHandlers
{
    public class PlayerPenaltyAddedEventHandler : INotificationHandler<DomainEventNotification<PlayerPenaltyAddedEvent>>
    {
        private readonly IFacebookPublisher _facebookPublisher;
        private readonly IApplicationDbContext _applicationDbContext;

        public PlayerPenaltyAddedEventHandler(IFacebookPublisher facebookPublisher, IApplicationDbContext applicationDbContext)
        {
            _facebookPublisher = facebookPublisher;
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(DomainEventNotification<PlayerPenaltyAddedEvent> notification, CancellationToken cancellationToken)
        {
            var player = await GetPlayerAsync(notification.DomainEvent.PlayerId, cancellationToken);
            var message = $"{player.Name.FirstName} {player.Name.LastName} +{Penalty.Unit}";

            await _facebookPublisher.PublishCommentAsync(message, cancellationToken);
        }

        private async Task<Player> GetPlayerAsync(int id, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .AsNoTracking()
                .FirstAsync(player => player.Id == id, cancellationToken);
    }
}