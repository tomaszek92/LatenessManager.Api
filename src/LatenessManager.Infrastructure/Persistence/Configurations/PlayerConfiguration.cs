using LatenessManager.Domain.Entities.PlayerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LatenessManager.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> playerBuilder)
        {
            playerBuilder.OwnsOne(x => x.Name, playerNameBuilder =>
            {
                playerNameBuilder.Property(x => x.FirstName).IsRequired();
                playerNameBuilder.Property(x => x.LastName).IsRequired();
            });
        }
    }
}