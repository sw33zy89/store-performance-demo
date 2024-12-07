using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTesting.Core.Services;
using PerformanceTesting.Infrastructure.Persistence;
using PerformanceTesting.Infrastructure.Services;

namespace PerformanceTesting.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuration
            services.AddSingleton<IConfiguration>(configuration);

            //Database
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("PerformanceTestDb"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DemoProjectDb") ?? "",
                        builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            }

            services.AddScoped<AppDbContextInitializer>();
            services.AddScoped<ICustomerService, CustomerService>();
            
            return services;
        }
    }
}
