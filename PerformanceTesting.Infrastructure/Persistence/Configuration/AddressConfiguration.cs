using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceTesting.Core;

namespace PerformanceTesting.Infrastructure.Persistence.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Address1)
                .HasColumnName("Address1")
                .IsRequired();

            builder.Property(x => x.Address2)
                .HasColumnName("Address2");

            builder.Property(x => x.City)
                .HasColumnName("City")
                .IsRequired();

            builder.Property(x => x.State)
                .HasColumnName("State");

            builder.Property(x => x.Country)
                .HasColumnName("Country")
                .IsRequired();

            builder.OwnsOne(x => x.Coordinates)
                .Property(x => x.Latitude)
                .HasColumnName("Latitude")
                .IsRequired();

            builder.OwnsOne(x => x.Coordinates)
                .Property(x => x.Longitude)
                .HasColumnName("Longitude")
                .IsRequired();
        }
    }
}
