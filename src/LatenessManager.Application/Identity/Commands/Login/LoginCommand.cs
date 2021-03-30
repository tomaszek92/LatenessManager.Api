using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Dtos;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.Login
{
    public class LoginCommand : IRequest<JsonWebTokenDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}