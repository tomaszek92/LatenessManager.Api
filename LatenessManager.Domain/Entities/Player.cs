using LatenessManager.Domain.Common;
using LatenessManager.Domain.ValueObjects;

namespace LatenessManager.Domain.Entities
{
    public class Player : BaseEntity, IAggregateRoot
    {
        public PlayerName Name { get; }

        public void AddPenalty()
        {
            
        }

        public void CarryOutPenalty()
        {
            
        }
    }
}