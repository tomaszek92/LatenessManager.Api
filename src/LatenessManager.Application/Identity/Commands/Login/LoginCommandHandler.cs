﻿using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Abstractions;
using MediatR;

namespace LatenessManager.Application.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JsonWebToken>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<JsonWebToken> Handle(LoginCommand request, CancellationToken cancellationToken) => 
            await _identityService.LoginAsync(request.Email, request.Password, cancellationToken);
    }
}