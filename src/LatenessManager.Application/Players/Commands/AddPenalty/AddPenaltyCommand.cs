using System;
using MediatR;

namespace LatenessManager.Application.Players.Commands.AddPenalty
{
    public class AddPenaltyCommand : IRequest
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
    }
}