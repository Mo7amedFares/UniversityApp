using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UniversityApp.Infrastructure.Persistence
{
    // هذا الكلاس تبحث عنه أداة EF Core تلقائياً قبل أن تذهب إلى Program.cs
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 1. تحديد مسار مشروع الـ API لقراءة ملف appsettings.json منه
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../UniversityApp.Api");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. قراءة نص الاتصال
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 3. بناء خيارات الـ DbContext وإخبار EF Core بمكان الـ Migrations
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseSqlServer(connectionString, b =>
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            return new ApplicationDbContext(builder.Options);
        }
    }
}