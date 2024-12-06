using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceTesting.Core;

namespace PerformanceTesting.Infrastructure.Persistence.Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.HasOne(x => x.Address);

            builder.HasIndex(x => x.Address);
        }
    }
}
