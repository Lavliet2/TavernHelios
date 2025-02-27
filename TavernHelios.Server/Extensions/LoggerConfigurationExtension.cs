using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System.Reflection;
using TavernHelios.AdministrationService.APICore.Settings;
using TavernHelios.Server.Formatters;

namespace TavernHelios.Server.Extensions
{
    public static class LoggerConfigurationExtension
    {
        public static LoggerConfiguration Configure(
            this LoggerConfiguration logger,
             //IOptions<RabbitMqSettings> options)
             IConfiguration configuration)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
            return logger
                .Enrich.WithProperty("Application", assemblyName) // Добавляем поле Application
                .Enrich.FromLogContext() // Для добавления дополнительных свойств из контекста
                .WriteTo.Console(new JsonFormatter(renderMessage: true))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(logEvent =>
                        logEvent.Properties.TryGetValue("SourceContext", out var value) &&
                        value.ToString() == "\"Middleware\"") // Фильтруем логи по контексту
                    .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                    {
                        clientConfiguration.Hostnames.Add(configuration["RABBITMQ_HOST"] ?? "localhost");
                        clientConfiguration.Username = configuration["RABBITMQ_USER"] ?? "admin";
                        clientConfiguration.Password = configuration["RABBITMQ_PASSWORD"] ?? "admin";
                        clientConfiguration.Exchange = "logs_exchange";
                        clientConfiguration.ExchangeType = "direct";
                        clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.Durable;
                        clientConfiguration.RoutingKey = "log";
                        clientConfiguration.Port =
                           Int32.TryParse(configuration["RABBITMQ_PORT"], out var res) ?
                           res :
                           5672;
                        clientConfiguration.VHost = "/";
                        clientConfiguration.Heartbeat = 60;

                        sinkConfiguration.TextFormatter = new LogJsonFormatter();
                    })
                );
        }
    }
}
