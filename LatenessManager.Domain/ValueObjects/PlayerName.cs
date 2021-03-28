using System.Collections.Generic;
using LatenessManager.Domain.Common;

namespace LatenessManager.Domain.ValueObjects
{
    public class PlayerName : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private PlayerName()
        {
            // EF Core required
        }

        public PlayerName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}