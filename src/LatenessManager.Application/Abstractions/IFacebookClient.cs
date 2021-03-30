using System.Threading;
using System.Threading.Tasks;

namespace LatenessManager.Application.Abstractions
{
    public interface IFacebookClient
    {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null, CancellationToken cancellationToken = default);
        Task PostAsync(string accessToken, string endpoint, object data, string args = null, CancellationToken cancellationToken = default);
    }
}