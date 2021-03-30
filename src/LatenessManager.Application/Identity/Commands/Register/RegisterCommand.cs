using MediatR;

namespace LatenessManager.Application.Identity.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}