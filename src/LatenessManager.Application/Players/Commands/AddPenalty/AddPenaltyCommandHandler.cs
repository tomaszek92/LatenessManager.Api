using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Domain.Entities.PlayerAggregate;
using LatenessManager.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.Commands.AddPenalty
{
    public class AddPenaltyCommandHandler : IRequestHandler<AddPenaltyCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AddPenaltyCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(AddPenaltyCommand request, CancellationToken cancellationToken)
        {
            var player = await GetPlayerAsync(request.Id, cancellationToken);
            player.AddPenalty(request.Date.Date);
            player.Events.Add(new PlayerPenaltyAddedEvent(request.Id));

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .FirstAsync(player => player.Id == playerId, cancellationToken);
    }
}