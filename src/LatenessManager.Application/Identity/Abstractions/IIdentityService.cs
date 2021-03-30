using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Dtos;

namespace LatenessManager.Application.Identity.Abstractions
{
    public interface IIdentityService
    {
        Task RegisterAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebTokenDto> LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task<JsonWebTokenDto> RefreshAccessTokenAsync(string token, CancellationToken cancellationToken);
        Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken);
    }
}