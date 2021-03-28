using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace LatenessManager.Application.Players.Commands.AddPenalty
{
    public class AddPenaltyCommandHandler : IRequestHandler<AddPenaltyCommand>
    {
        public Task<Unit> Handle(AddPenaltyCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}