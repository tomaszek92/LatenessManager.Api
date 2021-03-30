using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LatenessManager.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Players.Queries.GetPlayerById
{
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, PlayerDetailsDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetPlayerByIdQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<PlayerDetailsDto> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken) =>
            await _applicationDbContext
                .Players
                .AsNoTracking()
                .ProjectTo<PlayerDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(player => player.Id == request.Id, cancellationToken);
    }
}