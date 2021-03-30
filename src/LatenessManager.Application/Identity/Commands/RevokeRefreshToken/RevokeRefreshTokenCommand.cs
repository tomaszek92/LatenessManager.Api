using MediatR;

namespace LatenessManager.Application.Identity.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenCommand : IRequest
    {
        public string RefreshToken { get; set; }
    }
}