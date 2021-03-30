using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Identity.Abstractions;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
    {
        private readonly IIdentityService _identityService;

        public RevokeRefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _identityService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
            
            return Unit.Value;
        }
    }
}