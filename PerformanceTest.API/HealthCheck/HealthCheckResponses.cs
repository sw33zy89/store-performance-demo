namespace PerformanceTest.API.HealthCheck
{
    public class HealthCheckResponses
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckResponse> HealthChecks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}
