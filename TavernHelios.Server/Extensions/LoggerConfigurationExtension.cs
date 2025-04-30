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
    }
}
