using System;
using System.Collections.Generic;
using System.Linq;
using LatenessManager.Domain.Common;
using LatenessManager.Domain.ValueObjects;

namespace LatenessManager.Domain.Entities.PlayerAggregate
{
    public class Player : BaseEntity, IAggregateRoot
    {
        public PlayerName Name { get; }
        private readonly List<Penalty> _penalties = new();
        public IReadOnlyCollection<Penalty> Penalties => _penalties.AsReadOnly();

        public void AddPenalty(DateTime date)
        {
            if (_penalties.Any(p => p.Date == date && p.Count == Penalty.Unit))
            {
                throw new ArgumentException("Cannot add twice penalty at the same date.", nameof(date));
            }
            
            _penalties.Add(new Penalty(date, Penalty.Unit));
        }

        public void CarryOutPenalty(DateTime date)
        {
            _penalties.Add(new Penalty(date, -Penalty.Unit));
        }

        public void SetInitialPenalty(DateTime date, int count)
        {
            _penalties.Add(new Penalty(date, count));
        }
    }
}