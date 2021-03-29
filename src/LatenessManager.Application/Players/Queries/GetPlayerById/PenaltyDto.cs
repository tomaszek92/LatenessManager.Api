using System;
using LatenessManager.Application.Common.Mappings;
using LatenessManager.Domain.Entities.PlayerAggregate;

namespace LatenessManager.Application.Players.Queries.GetPlayerById
{
    public class PenaltyDto : IMapFrom<Penalty>
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}