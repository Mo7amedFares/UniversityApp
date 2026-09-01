using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notfication>
    {
        public void Configure(EntityTypeBuilder<Notfication> builder)
        {
            // منع الحذف المتسلسل بين المستخدم والإشعار
            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Request)
                   .WithMany(r => r.Notifications)
                   .HasForeignKey(n => n.RequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}