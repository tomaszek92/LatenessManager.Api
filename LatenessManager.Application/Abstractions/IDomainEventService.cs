using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Domain.Common;

namespace LatenessManager.Application.Abstractions
{
    public interface IDomainEventService
    {
        Task Publish(BaseDomainEvent baseDomainEvent, CancellationToken cancellationToken);
    }
}