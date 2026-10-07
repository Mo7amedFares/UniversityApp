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
                var context = services.GetRequiredService<ApplicationDbContext>();

                // استدعاء خدمة التشفير اللي مسجلها في الـ DI
                var passwordHasher = services.GetRequiredService<IPasswordHasher>();

                // 1. تطبيق التحديثات وبناء الجداول أولاً
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                // 2. التحقق وإضافة الإدمن ثانياً
                if (!context.Users.Any(u => u.Role == UserRole.admin))
                {
                    var adminUser = new User
                    {
                        Name = "مدير النظام",
                        NationalId = "1223334444555556",
                        PhoneNumber = "01200000000",
                        StudentCode = null,
                        Role = UserRole.admin,

                        // استخدام الخدمة الخاصة بك للتشفير (تأكد من اسم الدالة Hash أو حسب ما سميتها)
                        PasswordHash = passwordHasher.Hash("Admin@123")
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "حدث خطأ أثناء تحديث قاعدة البيانات أو إنشاء الإدمن.");
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