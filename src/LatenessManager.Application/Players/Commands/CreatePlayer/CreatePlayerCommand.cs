using MediatR;

namespace LatenessManager.Application.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int InitialPenaltyCount { get; set; }
    }
}