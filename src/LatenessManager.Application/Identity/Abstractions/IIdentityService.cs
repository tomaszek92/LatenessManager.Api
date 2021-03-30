using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;

namespace LatenessManager.Application.Identity.Abstractions
{
    public interface IIdentityService
    {
        Task RegisterAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebToken> LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebToken> RefreshAccessTokenAsync(string token, CancellationToken cancellationToken);
        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);
    }
}