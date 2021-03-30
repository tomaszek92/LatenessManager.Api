using System.Collections.Generic;
using MediatR;

namespace LatenessManager.Application.Players.Queries.GetPlayers
{
    public class GetPlayersQuery : IRequest<List<PlayerDto>>
    {
    }
}