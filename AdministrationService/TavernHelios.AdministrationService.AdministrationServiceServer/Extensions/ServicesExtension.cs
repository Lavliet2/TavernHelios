﻿using TavernHelios.AdministrationService.AdministrationServiceServer.BackgroundServices;
using TavernHelios.AdministrationService.AdministrationServiceServer.Services;
using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.ClickHouse.Services;
using TavernHelios.AdministrationService.RabbitMqConsumer.Services;
using TavernHelios.ClickHouse.Settings;
using TavernHelios.RabbitMq.Settings;

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

            services.AddSingleton<ClickHouseCleanupService>();

            services.AddSingleton<LogService>();

            services.AddHostedService<RabbitMqBackgroundService>();
            services.AddHostedService<ClickHouseCleanupBackgroundService>();
        }
    }
}
