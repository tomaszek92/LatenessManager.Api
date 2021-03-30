using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Identity.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        
        public RegisterCommandValidator(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress()
                .MustAsync(EmailIsUniqueAsync)
                .WithErrorCode(ErrorCode.User.EmailIsNotUnique);

            RuleFor(x => x.Password)
                .NotEmpty();
        }

        private async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
        {
            var emailLower = email.ToLower();

            return !await _applicationDbContext
                .Users
                .AnyAsync(user => user.Email == emailLower, cancellationToken);
        }
    }
}