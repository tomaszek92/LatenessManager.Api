using System.Collections.Generic;
using AutoMapper;
using LatenessManager.Application.Common.Mappings;
using LatenessManager.Domain.Entities.PlayerAggregate;

namespace LatenessManager.Application.Players.Queries.GetPlayerById
{
    public class PlayerDetailsDto : IMapFrom<Player>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PenaltyDto> Penalties { get; set; } = new();
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Player, PlayerDetailsDto>()
                .ForMember(
                    dest => dest.FirstName,
                    options => options.MapFrom(src => src.Name.FirstName))
                .ForMember(
                    dest => dest.LastName,
                    options => options.MapFrom(src => src.Name.LastName));
        }
    }
}