using System.Linq;
using AutoMapper;
using LatenessManager.Application.Common.Mappings;
using LatenessManager.Domain.Entities.PlayerAggregate;

namespace LatenessManager.Application.Players.Queries.GetPlayers
{
    public class PlayerDto : IMapFrom<Player>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PenaltiesCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Player, PlayerDto>()
                .ForMember(
                    dest => dest.FirstName,
                    options => options.MapFrom(src => src.Name.FirstName))
                .ForMember(
                    dest => dest.LastName,
                    options => options.MapFrom(src => src.Name.LastName))
                .ForMember(
                    dest => dest.PenaltiesCount,
                    options => options.MapFrom(src => src.Penalties.Sum(p => p.Count)));
        }
    }
}