using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Application.Identity.Dtos;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JsonWebTokenDto>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<JsonWebTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken) => 
            await _identityService.LoginAsync(request.Email, request.Password, cancellationToken);
    }
}