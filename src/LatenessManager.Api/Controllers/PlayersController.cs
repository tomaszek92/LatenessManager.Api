using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Players.Commands.AddPenalty;
using LatenessManager.Application.Players.Commands.CarryOutPenalty;
using LatenessManager.Application.Players.Commands.CreatePlayer;
using LatenessManager.Application.Players.Queries.GetPlayerById;
using LatenessManager.Application.Players.Queries.GetPlayers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    [Authorize]
    public class PlayersController : BaseController
    {
        public PlayersController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<PlayerDto>>> Get(CancellationToken cancellationToken)
        {
            var query = new GetPlayersQuery();
            var result = await Sender.Send(query, cancellationToken);

            return result;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PlayerDetailsDto>> GetById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var query = new GetPlayerByIdQuery(id);
            var result = await Sender.Send(query, cancellationToken);

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDetailsDto>> CreatePlayer(
            [FromBody] CreatePlayerCommand command,
            CancellationToken cancellationToken)
        {
            var id =await Sender.Send(command, cancellationToken);
            
            var query = new GetPlayerByIdQuery(id);
            var result = await Sender.Send(query, cancellationToken);
            
            return new CreatedAtRouteResult(nameof(GetById), result);
        }
        
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

            return new OkResult();
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