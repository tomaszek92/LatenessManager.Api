using AutoMapper;
using LatenessManager.Domain.Entities;

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