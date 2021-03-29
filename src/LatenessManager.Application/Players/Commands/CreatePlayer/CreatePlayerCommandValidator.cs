using FluentValidation;
using LatenessManager.Application.Common;
using LatenessManager.Domain.Entities.PlayerAggregate;

namespace LatenessManager.Application.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
    {
        public CreatePlayerCommandValidator()
        {
            RuleFor(x => x.InitialPenaltyCount)
                .Must(count => count >= 0 && count % Penalty.Unit == 0)
                .WithErrorCode(ErrorCode.Penalty.InvalidCount);
        }
    }
}