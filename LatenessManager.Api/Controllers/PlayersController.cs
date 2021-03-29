using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Players.Commands.AddPenalty;
using LatenessManager.Application.Players.Commands.CarryOutPenaltyCommand;
using LatenessManager.Application.Players.Queries.GetPlayerById;
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
        public async Task<ActionResult<PlayerDetailsDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetPlayerByIdQuery { Id = id };
            var result = await Sender.Send(query, cancellationToken);

            return result;
        }

        // [HttpPost]
        // public async Task<ActionResult> CreatePlayer(
        //     CancellationToken cancellationToken)
        // {
        //     throw new NotImplementedException();
        // }
        
        [HttpPost("{id}/penalty")]
        public async Task<ActionResult> AddPenalty(
            [FromRoute] int id,
            [FromBody] AddPenaltyCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                throw new ArgumentException("Inconsistent id", nameof(id));
            }
            
            await Sender.Send(command, cancellationToken);

            return new CreatedAtRouteResult(nameof(GetById), new { id });
        }
        
        [HttpDelete("{id}/penalty")]
        public async Task<ActionResult> CarryOutPenalty(
            [FromRoute] int id,
            [FromBody] CarryOutPenaltyCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                throw new ArgumentException("Inconsistent id", nameof(id));
            }
            
            await Sender.Send(command, cancellationToken);

            return new NoContentResult();
        }
    }
}