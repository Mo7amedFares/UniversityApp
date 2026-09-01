using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.NationalId)
                .IsUnique();

            builder.Property(u => u.NationalId)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(u => u.StudentCode)
                .IsUnique()
                .HasFilter("[StudentCode] IS NOT NULL");

            builder.Property(u => u.StudentCode)
                .HasMaxLength(20);

            builder.Property(u => u.Name).IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.PasswordHash).IsRequired();


        }
    }
}
