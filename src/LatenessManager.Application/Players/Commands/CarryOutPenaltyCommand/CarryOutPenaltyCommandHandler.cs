using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Domain.Entities.PlayerAggregate;
using LatenessManager.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.Commands.CarryOutPenaltyCommand
{
    public class CarryOutPenaltyCommandHandler : IRequestHandler<CarryOutPenaltyCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CarryOutPenaltyCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(CarryOutPenaltyCommand request, CancellationToken cancellationToken)
        {
            var player = await GetPlayerAsync(request.Id, cancellationToken);
            player.CarryOutPenalty(request.Date.Date);
            player.Events.Add(new PlayerPenaltyCarriedOutEvent(request.Id));

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
        
        private async Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .FirstAsync(p => p.Id == playerId, cancellationToken);
    }
}