using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace LatenessManager.Application.Players.Commands.CarryOutPenaltyCommand
{
    public class CarryOutPenaltyCommandHandler : IRequestHandler<CarryOutPenaltyCommand>
    {
        public Task<Unit> Handle(CarryOutPenaltyCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}