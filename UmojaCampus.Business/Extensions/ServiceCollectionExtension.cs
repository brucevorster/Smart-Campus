

using Microsoft.Extensions.DependencyInjection;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Business.Services.Implementations;

namespace UmojaCampus.Business.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQualificationService, QualificationService>();
            services.AddScoped<ISemesterService, SemesterService>();

            return services;
        }
    }
}
