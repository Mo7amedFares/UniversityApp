using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Entities;
namespace UniversityApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Request> Requests { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Notfication> Notifications { get; }
        DbSet<Service> Services { get; }
        DbSet<RequestNote> RequestNotes { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
