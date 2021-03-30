using System;

namespace LatenessManager.Domain.Common
{
    public abstract class BaseDomainEvent
    {
        public DateTime DateTimeOccurredUtc { get; } = DateTime.UtcNow;
    }
}