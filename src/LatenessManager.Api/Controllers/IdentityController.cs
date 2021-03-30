using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Identity.Commands.Login;
using LatenessManager.Application.Identity.Commands.RefreshAccessToken;
using LatenessManager.Application.Identity.Commands.Register;
using LatenessManager.Application.Identity.Commands.RevokeRefreshToken;
using LatenessManager.Application.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    [Authorize]
    public class IdentityController : BaseController
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public IdentityController(ISender sender, ICurrentUserProvider currentUserProvider) : base(sender)
        {
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
            var jsonWebTokenDto = await Sender.Send(command, cancellationToken);

            return jsonWebTokenDto;
        }

        [HttpPost("token/refresh")]
        public async Task<ActionResult<JsonWebTokenDto>> RefreshAccessToken(
            [FromBody] RefreshAccessTokenCommand command,
            CancellationToken cancellationToken)
        {
            var jsonWebTokenDto = await Sender.Send(command, cancellationToken);
            
            return jsonWebTokenDto;
        }
 
        [HttpPost("token/revoke")]
        public async Task<ActionResult> RevokeRefreshToken(
            [FromBody] RevokeRefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
 
            return new NoContentResult();
        }
    }
}