using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TavernHelios.RabbitMq.Helpers;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.RabbitMq.Services
{
    public abstract class RabbitMqBase : IDisposable
    {
        protected readonly string _connectionString;
        protected readonly IConnection _connection;
        protected readonly IChannel _channel;
        protected readonly string _queueName;
        protected readonly string _exchangeName;
        protected readonly string _routingKey;

        public RabbitMqBase(
            IOptions<RabbitMqSettings> settings,
            string queueName,
            (string exchangeName, string routingKey)? exchangeAndRoutingKey = null)
        {
            _connectionString = ConnectionHelper.GetRabbitMqConnectionString(settings.Value);

            _queueName = queueName;

            // Разбираем кортеж, если он передан
            if (exchangeAndRoutingKey.HasValue)
            {
                _exchangeName = exchangeAndRoutingKey.Value.exchangeName;
                _routingKey = exchangeAndRoutingKey.Value.routingKey;
            }
            else
            {
                _exchangeName = "";
                _routingKey = "";
            }

            var factory = new ConnectionFactory() { Uri = new Uri(_connectionString) };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

            DeclareAsync().Wait();

            async Task DeclareAsync()
            {
                // Объявляем обмен, если он указан
                if (!string.IsNullOrEmpty(_exchangeName))
                {
                    await _channel.ExchangeDeclareAsync(
                        exchange: _exchangeName,
                        type: ExchangeType.Direct,
                        durable: false,
                        autoDelete: false,
                        arguments: null);
                }

                // Объявляем очередь
                await _channel.QueueDeclareAsync(
                    queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Привязываем очередь к обмену, если обмен и routingKey указаны
                if (!string.IsNullOrEmpty(_exchangeName) && !string.IsNullOrEmpty(_routingKey))
                {
                    await _channel.QueueBindAsync(
                        queue: _queueName,
                        exchange: _exchangeName,
                        routingKey: _routingKey);
                }
            }
        }

        public void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
        }
    }
}
