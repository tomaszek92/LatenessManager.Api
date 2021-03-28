using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Domain.Common;
using MediatR;

namespace LatenessManager.Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly IPublisher _publisher;

        public DomainEventService(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Publish(BaseDomainEvent baseDomainEvent, CancellationToken cancellationToken)
        {
            await _publisher.Publish(baseDomainEvent, cancellationToken);
        }
    }
}