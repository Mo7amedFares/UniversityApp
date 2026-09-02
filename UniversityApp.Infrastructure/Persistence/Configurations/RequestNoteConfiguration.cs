using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class RequestNoteConfiguration : IEntityTypeConfiguration<RequestNote>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.RequestNote> builder)
        {
            builder.ToTable("RequestNotes");

            builder.HasKey(r => r.Id);

            builder.HasOne(n => n.User)
        .WithMany()
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Request)
                .WithMany(s => s.RequestNotes)
                .HasForeignKey(r => r.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
