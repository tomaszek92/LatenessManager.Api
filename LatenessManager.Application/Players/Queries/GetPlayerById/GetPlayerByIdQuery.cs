using MediatR;

namespace LatenessManager.Application.Players.Queries.GetPlayerById
{
    public class GetPlayerByIdQuery : IRequest<PlayerDetailsDto>
    {
        public int Id { get; init; }
    }
}