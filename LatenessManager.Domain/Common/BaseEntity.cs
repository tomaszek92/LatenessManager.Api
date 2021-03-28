using System.Collections.Generic;

namespace LatenessManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}