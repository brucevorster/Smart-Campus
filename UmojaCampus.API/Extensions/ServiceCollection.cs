using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UmojaCampus.Business.Data;
using UmojaCampus.Business.Persistence.Contexts;
using UmojaCampus.Shared.Configuration;

namespace UmojaCampus.API.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddConfiguration(configuration);
            services.AddDatabaseServices(configuration);           
            services.AddCorsPolicy();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfiguration();

            return services;
        }

        private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Umoja Campus API Endpoints", 
                    Version = "v1",
                    Description = "Provide a description here."
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

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
                        new string[] { }
                    }
                });
            });

            return services;
        }
        private static IServiceCollection AddDatabaseServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)))
                .AddTransient<IDatabaseSeeder, DatabaseSeeder>();

            return services;  
        }

        private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowUmojaCampusBlazorApp", 
                    builder => builder
                        .WithOrigins("https://localhost:7043", "http://localhost:5011")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));

            return services;
        }

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var jwtConfiguration = serviceProvider.GetRequiredService<IOptions<JwtConfiguration>>().Value;

            var secretKey = jwtConfiguration.SecretKey;
            var issuer = jwtConfiguration.ValidIssuer;
            var audience = jwtConfiguration.ValidAudience;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}
