using System.Collections.Generic;
using LatenessManager.Application.Players.Dtos;
using MediatR;

namespace LatenessManager.Application.Players.Queries.GetPlayers
{
    public class GetPlayersQuery : IRequest<List<PlayerDto>>
    {
    }
}