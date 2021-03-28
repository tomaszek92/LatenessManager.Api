using System;

namespace LatenessManager.Domain.Common
{
    public abstract class BaseDomainEvent
    {
        public DateTimeOffset DateOccurred { get; } = DateTime.UtcNow;
    }
}