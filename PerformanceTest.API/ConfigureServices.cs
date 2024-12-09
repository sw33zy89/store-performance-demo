using PerformanceTest.API.HealthCheck;
using PerformanceTesting.Infrastructure.Persistence;
using PerformanceTesting.Infrastructure.Services;

namespace PerformanceTest.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole(); // Add logging providers like Console, Debug, File, etc.
            });

            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>()
                .AddCheck<SampleHealthCheck>("SampleHealthCheck");

            // Add services to the container.
            services.AddDistributedMemoryCache();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.AddHttpClient();
            services.AddHttpClient<OpenMeteoClient>();

            return services;
        }
    }
}
