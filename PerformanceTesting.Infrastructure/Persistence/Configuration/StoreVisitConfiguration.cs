using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PerformanceTesting.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTesting.Infrastructure.Persistence.Configuration
{
    public class StoreVisitConfiguration : IEntityTypeConfiguration<StoreVisit>
    {
        public void Configure(EntityTypeBuilder<StoreVisit> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.VisitDateTime)
                .HasColumnName("VisitDateTime")
                .IsRequired();

            builder.HasOne(x => x.Store)
                .WithMany(x => x.CustomerVisits)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.StoreVisits)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasIndex(x => x.StoreId);
            builder.HasIndex(x => x.CustomerId);
        }
    }
}
