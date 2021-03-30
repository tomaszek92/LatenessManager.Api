using LatenessManager.Application.Common.Models;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.Login
{
    public class LoginCommand : IRequest<JsonWebToken>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}