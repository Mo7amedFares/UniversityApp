using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // تحديد دقة الأرقام المالية (18 رقم إجمالاً، منها 2 بعد العلامة العشرية)
            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)");
        }
    }
}