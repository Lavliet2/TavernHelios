using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using TavernHelios.RabbitMq.Settings;
using Polly;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace TavernHelios.RabbitMq.Helpers
{
    public class ConnectionHelper
    {
        public static string GetRabbitMqConnectionString(RabbitMqSettings settings)
        {
            return $"amqp://{settings.User}:{settings.Password}@{settings.Host}:{settings.Port}/";
        }

        public static async Task<IConnection> CreateRabbitMqConnection(
            RabbitMqSettings settings,
            ILogger logger)
        {
            var factory = new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                UserName = settings.User,
                Password = settings.Password
            };

            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(
                    10, _ => TimeSpan.FromSeconds(3),
                    (ex, ts, attempt, ctx) =>
                    {
                        logger.LogWarning("Не удалось подключиться к RabbitMQ (попытка {attempt}): {msg}", attempt, ex.Message);
                        return Task.CompletedTask;
                    });

            return await policy.ExecuteAsync(async () =>
            {
                return await factory.CreateConnectionAsync();
            });
        }
    }
}
