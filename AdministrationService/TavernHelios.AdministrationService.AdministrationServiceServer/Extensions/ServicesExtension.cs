using TavernHelios.AdministrationService.AdministrationServiceServer.BackgroundServices;
using TavernHelios.AdministrationService.APICore.Settings;
using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.RabbitMqConsumer.Services;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClickHouseSettings>(configuration.GetSection("ClickHouse"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));

            services.AddSingleton<LogRepository>();
            services.AddSingleton<LogConsumerService>();

            services.AddHostedService<RabbitMqBackgroundService>();
            services.AddHostedService<ClickHouseCleanupBackgroundService>();
        }
    }
}
