using LatenessManager.Application.Identity.Dtos;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommand : IRequest<JsonWebTokenDto>
    {
        public string RefreshToken { get; set; }
    }
}