using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.Commands.CarryOutPenaltyCommand
{
    public class CarryOutPenaltyCommandValidator : AbstractValidator<CarryOutPenaltyCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        
        public CarryOutPenaltyCommandValidator(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            RuleFor(x => x.Id)
                .MustAsync(PlayerExistAsync)
                .WithErrorCode(ErrorCode.Player.NotExist);
        }

        private async Task<bool> PlayerExistAsync(int playerId, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .AnyAsync(player => player.Id == playerId, cancellationToken);
    }
}