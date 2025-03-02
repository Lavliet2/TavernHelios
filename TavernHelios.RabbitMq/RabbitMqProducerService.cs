using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.RabbitMq.Services
{
    public class RabbitMqProducerService<T> : RabbitMqBase
    {

        public delegate byte[] SerializeMessageHandler(T message);
        public delegate byte[] SerializeMessagesHandler(IEnumerable<T> message);

        /// <summary>
        /// Делегат для десериализации
        /// </summary>
        public event SerializeMessageHandler? SerializeMessage;
        public event SerializeMessagesHandler? SerializeMessages;

        public RabbitMqProducerService(
            IOptions<RabbitMqSettings> settings,
            string queueName,
            (string exchangeName, string routingKey)? exchangeAndRoutingKey = null)
            : base(settings, queueName, exchangeAndRoutingKey)
        {
        }

        public async Task SendMessage(T message)
        {
            var body = SerializeMessage?.Invoke(message);

            await _channel.BasicPublishAsync(
                exchange: string.IsNullOrEmpty(_exchangeName) ? "" : _exchangeName,
                routingKey: string.IsNullOrEmpty(_routingKey) ? _queueName : _routingKey,
                body: body);
        }

        public async Task SendMessages(IEnumerable<T> messages)
        {
            // Сериализация коллекции в MessagePack
            var body = SerializeMessages?.Invoke(messages);

            // Отправка сообщения в RabbitMQ
            await _channel.BasicPublishAsync(
                exchange: string.IsNullOrEmpty(_exchangeName) ? "" : _exchangeName,
                routingKey: string.IsNullOrEmpty(_routingKey) ? _queueName : _routingKey,
                body: body);
        }
    }
}
