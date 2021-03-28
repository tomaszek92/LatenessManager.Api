using AutoMapper;
using LatenessManager.Domain.Entities;
using LatenessManager.Domain.Entities.PlayerAggregate;

namespace LatenessManager.Application.Players.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
        }
    }
}