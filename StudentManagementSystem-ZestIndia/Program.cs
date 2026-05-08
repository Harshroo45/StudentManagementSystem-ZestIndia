using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StudentManagementSystem_ZestIndia.Data;
using StudentManagementSystem_ZestIndia.Helpers;
using StudentManagementSystem_ZestIndia.Middleware;
using StudentManagementSystem_ZestIndia.Repositories.Implementations;
using StudentManagementSystem_ZestIndia.Repositories.Interfaces;
using StudentManagementSystem_ZestIndia.Services.Implementations;
using StudentManagementSystem_ZestIndia.Services.Interfaces;
using System.Text;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to container
    builder.Services.AddControllers();

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "StudentManagementSystem-ZestIndia",
            Version = "v1"
        });

        options.AddSecurityDefinition("Bearer",
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter JWT Token"
            });

        options.AddSecurityRequirement(
            new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
            });
    });

    // Configure DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

    // Configure JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    var secret = jwtSettings["Secret"];
    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    if (string.IsNullOrEmpty(secret) || secret.Length < 32)
    {
        throw new InvalidOperationException(
            "JWT Secret is not configured properly. " +
            "Ensure it's at least 32 characters long in appsettings.json");
    }

    var key = Encoding.UTF8.GetBytes(secret);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

    builder.Services.AddAuthorization();

    // Register Repository Services
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();

    // Register Business Services
    builder.Services.AddScoped<IStudentService, StudentService>();

    // Register Helper Services
    builder.Services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();

    // Configure CORS
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

    // Apply migrations automatically
    using (var scope = app.Services.CreateScope())
    {
        var dbContext =
            scope.ServiceProvider
                 .GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    // Configure Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();

        app.UseSwaggerUI();
    }

    // Global Exception Middleware
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    // Authentication & Authorization
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}