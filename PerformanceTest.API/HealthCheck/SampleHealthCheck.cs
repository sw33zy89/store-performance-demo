using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PerformanceTest.API.HealthCheck
{
    public class SampleHealthCheck : IHealthCheck
    {
        private readonly ILogger<SampleHealthCheck> _logger;

        public SampleHealthCheck(ILogger<SampleHealthCheck> logger)
        {
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            bool isHealthy;
            try
            {
                //DO SOME LOGIC
                isHealthy = true;
            }
            catch (Exception ex)
            {
                isHealthy = false;

                _logger.LogError(ex, "Sample healthcheck failed.");
            }

            if (isHealthy)
            {
                return HealthCheckResult.Healthy("The sample healthcheck is healthy.");
            }

            return new HealthCheckResult(
                    context.Registration.FailureStatus, "The sample healtcheck failed.");
        }
    }
}
