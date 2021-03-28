﻿using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Microsoft.EntityFrameworkCore;

namespace LatenessManager.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<Player> Players { get; set; }
        DbSet<Penalty> Penalties { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}