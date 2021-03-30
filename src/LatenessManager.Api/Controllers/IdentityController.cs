using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Application.Identity.Commands.Login;
using LatenessManager.Application.Identity.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityController(
            ISender sender,
            IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor) : base(sender)
        {
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> Get() => 
            _httpContextAccessor.HttpContext?.User.Identity?.Name;

        [HttpPost("register")]
        public async Task<ActionResult> Register(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            await Sender.Send(command, cancellationToken);
        
            return new NoContentResult();
        }

        [HttpPost("login")]
        public async Task<ActionResult<JsonWebToken>> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var jsonWebToken = await Sender.Send(command, cancellationToken);

            return jsonWebToken;
        }

        [HttpPost("tokens/{token}/refresh")]
        [Authorize]
        public async Task<ActionResult<JsonWebToken>> RefreshAccessToken(
            string token,
            CancellationToken cancellationToken)
        {
            var jsonWebToken = await _identityService.RefreshAccessTokenAsync(token, cancellationToken);

            return jsonWebToken;
        }
 
        [HttpPost("tokens/{token}/revoke")]
        [Authorize]
        public async Task<ActionResult> RevokeRefreshToken(
            string token,
            CancellationToken cancellationToken)
        {
            await _identityService.RevokeRefreshTokenAsync(token, cancellationToken);
 
            return new NoContentResult();
        }
    }
}