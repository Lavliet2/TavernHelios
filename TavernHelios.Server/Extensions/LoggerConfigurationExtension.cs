using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System.Reflection;
using TavernHelios.Server.Formatters;

namespace TavernHelios.Server.Extensions
{
    public static class LoggerConfigurationExtension
    {
        public static LoggerConfiguration Configure(
    this LoggerConfiguration logger,
    IConfiguration configuration)
        {
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

            var rabbitSection = configuration.GetSection("RabbitMQ");
            var host = rabbitSection["Host"] ?? "localhost";
            var port = int.TryParse(rabbitSection["Port"], out var parsedPort) ? parsedPort : 5672;
            var user = rabbitSection["User"] ?? "admin";
            var password = rabbitSection["Password"] ?? "admin";

            return logger
                .Enrich.WithProperty("Application", assemblyName)
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(logEvent =>
                        logEvent.Properties.TryGetValue("SourceContext", out var value) &&
                        value.ToString() == "\"Middleware\"")
                    .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                    {
                        clientConfiguration.Hostnames.Add(host);
                        clientConfiguration.Username = user;
                        clientConfiguration.Password = password;
                        clientConfiguration.Exchange = "logs_exchange";
                        clientConfiguration.ExchangeType = "direct";
                        clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.Durable;
                        clientConfiguration.RoutingKey = "log";
                        clientConfiguration.Port = port;
                        clientConfiguration.VHost = "/";
                        clientConfiguration.Heartbeat = 60;

                        sinkConfiguration.TextFormatter = new LogJsonFormatter();
                    })
                );
        }

        //public static LoggerConfiguration Configure(
        //    this LoggerConfiguration logger,
        //     IConfiguration configuration)
        //{
        //    var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        //    return logger
        //        .Enrich.WithProperty("Application", assemblyName) // Добавляем поле Application
        //        .Enrich.FromLogContext() // Для добавления дополнительных свойств из контекста
        //        //.WriteTo.Console(new JsonFormatter(renderMessage: true))
        //        .WriteTo.Logger(lc => lc
        //            .Filter.ByIncludingOnly(logEvent =>
        //                logEvent.Properties.TryGetValue("SourceContext", out var value) &&
        //                value.ToString() == "\"Middleware\"") // Фильтруем логи по контексту
        //            .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
        //            {
        //                clientConfiguration.Hostnames.Add(configuration["RABBITMQ_HOST"] ?? "localhost");
        //                clientConfiguration.Username = configuration["RABBITMQ_USER"] ?? "admin";
        //                clientConfiguration.Password = configuration["RABBITMQ_PASSWORD"] ?? "admin";
        //                clientConfiguration.Exchange = "logs_exchange";
        //                clientConfiguration.ExchangeType = "direct";
        //                clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.Durable;
        //                clientConfiguration.RoutingKey = "log";
        //                clientConfiguration.Port =
        //                   Int32.TryParse(configuration["RABBITMQ_PORT"], out var res) ?
        //                   res :
        //                   5672;
        //                clientConfiguration.VHost = "/";
        //                clientConfiguration.Heartbeat = 60;

        //                sinkConfiguration.TextFormatter = new LogJsonFormatter();
        //            })
        //        );
        //}
    }
}
