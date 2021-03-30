using System.Threading;
using System.Threading.Tasks;

namespace LatenessManager.Application.Abstractions
{
    public interface IFacebookPublisher
    {
        Task PublishCommentAsync(string message, CancellationToken cancellationToken);
    }
}