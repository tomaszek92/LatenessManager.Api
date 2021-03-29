using System;
using MediatR;

namespace LatenessManager.Application.Players.Commands.CarryOutPenaltyCommand
{
    public class CarryOutPenaltyCommand : IRequest
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
    }
}