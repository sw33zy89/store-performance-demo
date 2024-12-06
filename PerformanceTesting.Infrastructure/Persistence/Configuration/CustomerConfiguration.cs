using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceTesting.Core;

namespace PerformanceTesting.Infrastructure.Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Name)
                .Property(x => x.FirstName)
                .HasColumnName("FirstName");

            builder.OwnsOne(x => x.Name)
                .Property(x => x.LastName)
                .HasColumnName("LastName");

            builder.HasMany(x => x.Addresses)
                .WithMany();
        }
    }
}
