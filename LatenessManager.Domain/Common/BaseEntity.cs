using System.Collections.Generic;

namespace LatenessManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }

        public readonly List<BaseDomainEvent> Events = new();
    }
}