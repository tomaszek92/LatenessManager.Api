using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Domain.Entities.PlayerAggregate;
using LatenessManager.Domain.ValueObjects;
using MediatR;

namespace LatenessManager.Application.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, int>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreatePlayerCommandHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var player = new Player(new PlayerName(request.FirstName, request.LastName));
            player.SetInitialPenalty(_dateTimeProvider.UtcNow.ToLocalTime().Date, request.InitialPenaltyCount);

            await _applicationDbContext.Players.AddAsync(player, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return player.Id;
        }
    }
}