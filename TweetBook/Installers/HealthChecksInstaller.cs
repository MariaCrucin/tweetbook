using TweetBook.Data;
using TweetBook.HealthChecks;

namespace TweetBook.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<DataContext>()
                .AddCheck<RedisHealthCheck>("Redis");
        }
    }
}
