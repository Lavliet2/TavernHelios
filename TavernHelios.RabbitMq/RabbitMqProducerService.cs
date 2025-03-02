using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.RabbitMq.Services
{
    public class RabbitMqProducerService<T> : RabbitMqBase
    {
        // Делегат для сериализации
        private event Func<T, byte[]> _serialize;

        public RabbitMqProducerService(
            IOptions<RabbitMqSettings> settings,
            Func<T, byte[]> serialize,
            string queueName,
            (string exchangeName, string routingKey)? exchangeAndRoutingKey = null)
            : base(settings, queueName, exchangeAndRoutingKey)
        {
            _serialize = serialize;
        }

        public async Task SendMessage(T message)
        {
            var body = _serialize(message);

            await _channel.BasicPublishAsync(
                exchange: string.IsNullOrEmpty(_exchangeName) ? "" : _exchangeName,
                routingKey: string.IsNullOrEmpty(_routingKey) ? _queueName : _routingKey,
                body: body);
        }
    }
}
