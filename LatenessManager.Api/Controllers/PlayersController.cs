using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Players.Dtos;
using LatenessManager.Application.Players.Queries.GetPlayers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    public class PlayersController : BaseController
    {
        public PlayersController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<PlayerDto>>> Get(CancellationToken cancellationToken)
        {
            var query = new GetPlayersQuery();
            var result = await Sender.Send(query, cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}