using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Application.Identity.Dtos;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, JsonWebTokenDto>
    {
        private readonly IIdentityService _identityService;

        public RefreshAccessTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<JsonWebTokenDto> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken) => 
            await _identityService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);
    }
}