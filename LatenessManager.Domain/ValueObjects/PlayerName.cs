using System.Collections.Generic;
using LatenessManager.Domain.Common;

namespace LatenessManager.Domain.ValueObjects
{
    public class PlayerName : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }

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