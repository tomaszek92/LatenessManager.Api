using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Identity.Abstractions;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _identityService.RegisterAsync(request.Email, request.Password, cancellationToken);
            
            return Unit.Value;
        }
    }
}