using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Application.Contracts.Persistence;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HakimHubDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
          
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();
            services.AddScoped<IInstitutionAvailabilityRepository, InstitutionAvailabilityRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }

    }
}