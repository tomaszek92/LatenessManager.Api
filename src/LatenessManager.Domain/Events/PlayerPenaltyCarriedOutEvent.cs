using LatenessManager.Domain.Common;

namespace LatenessManager.Domain.Events
{
    public class PlayerPenaltyCarriedOutEvent : BaseDomainEvent
    {
        public int PlayerId { get; }

        public PlayerPenaltyCarriedOutEvent(int playerId)
        {
            PlayerId = playerId;
        }
    }
}