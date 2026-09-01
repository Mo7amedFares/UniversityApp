using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();

        public DbSet<Request> Requests => Set<Request>();

        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<Notfication> Notifications => Set<Notfication>();

        public DbSet<Service> Services => Set<Service>();

        public DbSet<RequestNote> RequestNotes => Set<RequestNote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
