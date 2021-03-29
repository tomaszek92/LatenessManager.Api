using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LatenessManager.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<Player> Players { get; set; }
        DbSet<Penalty> Penalties { get; set; }
        
        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}