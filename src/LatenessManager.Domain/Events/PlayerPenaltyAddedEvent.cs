using LatenessManager.Domain.Common;

namespace LatenessManager.Domain.Events
{
    public class PlayerPenaltyAddedEvent : BaseDomainEvent
    {
        public int PlayerId { get; }

        public PlayerPenaltyAddedEvent(int playerId)
        {
            PlayerId = playerId;
        }
    }
}