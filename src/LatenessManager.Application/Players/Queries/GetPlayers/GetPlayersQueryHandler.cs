using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LatenessManager.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.Queries.GetPlayers
{
    public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, List<PlayerDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetPlayersQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<List<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
    }
}