using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(s => s.Fees)
                   .HasColumnType("decimal(18,2)");
        }
    }
}