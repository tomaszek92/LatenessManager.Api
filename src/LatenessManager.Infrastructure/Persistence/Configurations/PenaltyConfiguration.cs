using LatenessManager.Domain.Entities.PlayerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LatenessManager.Infrastructure.Persistence.Configurations
{
    public class PenaltyConfiguration : IEntityTypeConfiguration<Penalty>
    {
        public void Configure(EntityTypeBuilder<Penalty> builder)
        {
            builder.Property(x => x.Date).HasColumnType("date");
        }
    }
}