using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;

namespace LatenessManager.Application.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task RegisterAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebToken> LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebToken> RefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task RevokeTokenAsync(string token, CancellationToken cancellationToken);
    }
}