using System;
using LatenessManager.Domain.Common;

namespace LatenessManager.Domain.Entities.PlayerAggregate
{
    public class Penalty : BaseEntity
    {
        public const int Unit = 100;

        public DateTime Date { get; private set; }
        public int Count { get; private set; }
        public int PlayerId { get; private set; }

        public Penalty(DateTime date, int count)
        {
            if (date == default)
            {
                throw new ArgumentException("Date cannot be default", nameof(date));
            }

            Date = date;
            Count = count;
        }
    }
}