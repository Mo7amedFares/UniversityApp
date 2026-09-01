using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
using UniversityApp.Api.ExceptionHandlers;
using UniversityApp.Application.Common.Behaviors;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Application.Features.Services.Queries.GetAllServices;
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

        var app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); // 1. من أنت؟ (فك التوكن)
        app.UseAuthorization();  // 2. هل مسموح لك بالدخول؟ (الصلاحيات)

        // 2. السطر المفقود الثاني: توجيه الطلبات (Routing) للـ Controllers
        app.MapControllers();

        app.Run();
    }
}