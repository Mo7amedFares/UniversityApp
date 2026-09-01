using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Request> builder)
        {
            builder.ToTable("Requests");

            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Student)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Service)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
