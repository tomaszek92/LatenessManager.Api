using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Application.Identity.Commands.Login;
using LatenessManager.Application.Identity.Commands.Register;
using LatenessManager.Application.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    [Authorize]
    public class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserProvider _currentUserProvider;

        public IdentityController(
            ISender sender,
            IIdentityService identityService, ICurrentUserProvider currentUserProvider) : base(sender)
        {
            _identityService = identityService;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        public ActionResult<int> Get() => _currentUserProvider.Id;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
        
            return new NoContentResult();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<JsonWebTokenDto>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var jsonWebToken = await Sender.Send(command, cancellationToken);

            return jsonWebToken;
        }

        [HttpPost("tokens/{token}/refresh")]
        public async Task<ActionResult<JsonWebTokenDto>> RefreshAccessToken(
            string token,
            CancellationToken cancellationToken)
        {
            var jsonWebToken = await _identityService.RefreshAccessTokenAsync(token, cancellationToken);

            return jsonWebToken;
        }
 
        [HttpPost("tokens/{token}/revoke")]
        public async Task<ActionResult> RevokeRefreshToken(
            string token,
            CancellationToken cancellationToken)
        {
            await _identityService.RevokeRefreshTokenAsync(token, cancellationToken);
 
            return new NoContentResult();
        }
    }
}