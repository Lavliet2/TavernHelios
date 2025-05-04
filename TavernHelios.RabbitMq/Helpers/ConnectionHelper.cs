using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using TavernHelios.RabbitMq.Settings;
using Polly;
using Microsoft.Extensions.Logging;

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
                Password = settings.Password, 
            };

            // Add retry logic for Docker startup delays
            var policy = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return await policy.Execute(() => {
                var connection = factory.CreateConnectionAsync();
                logger.LogInformation($"Выполнено подключение к {factory.Uri}");
                return connection;
                });
        }
    }
}
