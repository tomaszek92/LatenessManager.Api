using System;

namespace LatenessManager.Domain.Common
{
    public abstract class BaseDomainEvent
    {
        public DateTime DateOccurred { get; } = DateTime.UtcNow;
    }
}