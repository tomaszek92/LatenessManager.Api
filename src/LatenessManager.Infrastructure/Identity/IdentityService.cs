using System;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Abstractions.Identity;
using LatenessManager.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IApplicationDbContext _applicationDbContext;

        public IdentityService(
            IJwtHandler jwtHandler,
            IPasswordHasher<User> passwordHasher,
            IApplicationDbContext applicationDbContext)
        {
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _applicationDbContext = applicationDbContext;
        }

        public async Task RegisterAsync(string email, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception($"Username can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"Password can not be empty.");
            }
            if (await GetUserAsync(email, cancellationToken) != null)
            {
                throw new Exception($"Email '{email}' is already in use.");
            }
            
            await AddUserToDatabaseAsync(email, password, cancellationToken);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(email, cancellationToken);
            
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }
            
            var jwt = _jwtHandler.Create(user.Id);

            var refreshToken = _passwordHasher.HashPassword(user, password);
            
            jwt.RefreshToken = refreshToken;
            
            await AddUserTokenToDatabaseAsync(user.Id, refreshToken, cancellationToken);
 
            return jwt;
        }

        public async Task<JsonWebToken> RefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await GetUserTokenAsync(token, cancellationToken);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }

            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was revoked");
            }

            var jwt = _jwtHandler.Create(refreshToken.UserId);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public async Task RevokeTokenAsync(string token, CancellationToken cancellationToken)
        {
            var userToken = await GetUserTokenAsync(token, cancellationToken);
            if (userToken == null)
            {
                throw new Exception("Token was not found.");
            }

            if (userToken.Revoked)
            {
                throw new Exception("Token was already revoked.");
            }

            userToken.Revoked = true;
        }

        private async Task<User> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var emailLower = email.ToLower();
            
            return await _applicationDbContext
                .Users
                .FirstOrDefaultAsync(user => user.Email == emailLower, cancellationToken);
        }

        private async Task<UserToken> GetUserTokenAsync(string token, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .UserTokens
                .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token, cancellationToken);

        private async Task AddUserToDatabaseAsync(
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = email.ToLower(),
                Password = password
            };

            await _applicationDbContext.Users.AddAsync(user, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        
        private async Task AddUserTokenToDatabaseAsync(
            int userId, 
            string token,
            CancellationToken cancellationToken)
        {
            var userToken = new UserToken
            {
                UserId = userId,
                Token = token
            };
            await _applicationDbContext.UserTokens.AddAsync(userToken, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}