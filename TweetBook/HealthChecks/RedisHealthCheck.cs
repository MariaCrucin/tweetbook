using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace TweetBook.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _connectionMupltiplexer;

        public RedisHealthCheck(IConnectionMultiplexer connectionMupltiplexer)
        {
            _connectionMupltiplexer = connectionMupltiplexer;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = _connectionMupltiplexer.GetDatabase();
                database.StringGet("health");
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(exception.Message));
            }
        }
    }
}
