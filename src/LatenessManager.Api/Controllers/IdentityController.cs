using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Api.Models.Identity;
using LatenessManager.Application.Abstractions.Identity;
using LatenessManager.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController
    {
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityController(IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
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
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            await _identityService.RegisterAsync(request.Email, request.Password, cancellationToken);
        
            return new NoContentResult();
        }

        [HttpPost("login")]
        public async Task<ActionResult<JsonWebToken>> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var jsonWebToken = await _identityService.LoginAsync(request.Email, request.Password, cancellationToken);

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