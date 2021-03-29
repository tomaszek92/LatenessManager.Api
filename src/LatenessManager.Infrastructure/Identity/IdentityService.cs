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

            var user = new User
            {
                Email = email
            };

            var hashPassword = _passwordHasher.HashPassword(user, password);
            user.Password = hashPassword;
            
            await AddUserToDatabaseAsync(user, cancellationToken);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(email, cancellationToken) ??
                       throw new Exception("Invalid credentials.");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid credentials.");
            }

            var jwt = _jwtHandler.Create(user.Id);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString());
            jwt.RefreshToken = refreshToken;
            
            await AddRefreshTokenToDatabaseAsync(user.Id, refreshToken, cancellationToken);
 
            return jwt;
        }

        public async Task<JsonWebToken> RefreshAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await GetRefreshTokenAsync(token, cancellationToken) ??
                               throw new Exception("Refresh token was not found.");

            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was revoked");
            }

            var jwt = _jwtHandler.Create(refreshToken.UserId);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await GetRefreshTokenAsync(token, cancellationToken) ??
                               throw new Exception("Refresh token was not found.");

            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was already revoked.");
            }

            refreshToken.Revoked = true;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<User> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var emailLower = email.ToLower();
            
            return await _applicationDbContext
                .Users
                .FirstOrDefaultAsync(user => user.Email == emailLower, cancellationToken);
        }

        private async Task<RefreshToken> GetRefreshTokenAsync(string token, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .RefreshTokens
                .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token, cancellationToken);

        private async Task AddUserToDatabaseAsync(
            User user,
            CancellationToken cancellationToken)
        {
            await _applicationDbContext.Users.AddAsync(user, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        
        private async Task AddRefreshTokenToDatabaseAsync(
            int userId, 
            string token,
            CancellationToken cancellationToken)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token
            };
            await _applicationDbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}