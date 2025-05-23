using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace TavernHelios.WeatherServiceServer.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureWeatherServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();
        services.AddHttpClient();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
    }
}
