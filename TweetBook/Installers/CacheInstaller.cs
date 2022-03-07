using StackExchange.Redis;
using TweetBook.Cache;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var redisCacheSettings = new RedisCacheSettings();
            builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            builder.Services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled)
            {
                return;
            }

            builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));
            builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
