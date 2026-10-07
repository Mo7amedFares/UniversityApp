using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
using UniversityApp.Api.ExceptionHandlers;
using UniversityApp.Api.Services;
using UniversityApp.Application.Common.Behaviors;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Features.Services.Queries.GetAllServices;
using UniversityApp.Domain.Entities;
using UniversityApp.Domain.Enums;
using UniversityApp.Infrastructure.Authentication;
using UniversityApp.Infrastructure.Persistence;
using UniversityApp.Infrastructure.Services;
public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. السطر المفقود الأول: إخبار النظام بتفعيل الـ Controllers
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // تعريف نوع الحماية (Bearer Token)
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "أدخل التوكن هنا مباشرة"
            });

            // فرض الحماية على الـ Endpoints
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddMediatR(cfg =>
        {

            cfg.RegisterServicesFromAssembly(typeof(GetAllServicesQuery).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(typeof(GetAllServicesQuery).Assembly);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        // تسجيل خدمة توليد التوكن
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        builder.Services.AddScoped<IFileService, FileService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

        // هذا السطر ضروري جداً لكي يعمل IHttpContextAccessor
        builder.Services.AddHttpContextAccessor();

        // تسجيل خدمتنا
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        // أضف هذا السطر مع باقي تسجيلات الـ Services
        builder.Services.AddScoped<IPaymentService, MockPaymentService>();

        // أضف هذا مع تسجيل الخدمات (Services)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // استبدل ApplicationDbContext باسم الـ DbContext الفعلي عندك
                var context = services.GetRequiredService<ApplicationDbContext>();

                // لو بتستخدم خدمة لتشفير الباسورد استدعيها هنا (مثال)
                // var passwordHasher = services.GetRequiredService<IPasswordHasher>();

                // التأكد من عدم وجود أي مستخدم بصلاحية أدمن
                if (!context.Users.Any(u => u.Role == UserRole.admin))
                {
                    var adminUser = new User
                    {
                        Name = "مدير النظام",
                        NationalId = "11112222333344", // حقل إجباري في الموديل
                        PhoneNumber = "01000000000",   // حقل إجباري في الموديل
                        StudentCode = null,            // الأدمن ليس له كود طالب
                        Role = UserRole.admin,

                        // يجب تشفير الباسورد قبل حفظه بناءً على الـ Service الخاصة بك
                        // PasswordHash = passwordHasher.Hash("Admin@123") 
                        PasswordHash = "Admin@123" // غيرها لتستخدم دالة التشفير الخاصة بك
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "حدث خطأ أثناء إنشاء حساب الإدمن الافتراضي.");
            }
        }

        // أضف هذا في الـ Pipeline قبل app.UseAuthentication()
        app.UseCors("AllowAll");
        app.UseStaticFiles(); 

        app.UseExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication(); // 1. من أنت؟ (فك التوكن)
        app.UseAuthorization();  // 2. هل مسموح لك بالدخول؟ (الصلاحيات)

        // 2. السطر المفقود الثاني: توجيه الطلبات (Routing) للـ Controllers
        app.MapControllers();
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // يفضل تسجيل الخطأ هنا باستخدام ILogger
                Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
            }
        }

        app.Run();
    }
}