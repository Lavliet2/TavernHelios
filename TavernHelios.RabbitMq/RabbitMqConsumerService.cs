using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.RabbitMq.Services
{
    public class RabbitMqConsumerService<T> : RabbitMqBase where T : class
    {
        public delegate T? DeserializeHandler(byte[] body);
        public delegate Task<T?> DoHandler(T? message);

        /// <summary>
        /// Делегат для десериализации
        /// </summary>
        public event DeserializeHandler? Deserialize;

        /// <summary>
        /// Действие над десериализованным сообщением. 
        /// Например, запись в БД
        /// </summary>
        public event DoHandler? DoAsync;

        public RabbitMqConsumerService(
            IOptions<RabbitMqSettings> settings,
            ILogger<RabbitMqConsumerService<T>> logger,
            string queueName,
            (string exchangeName, string routingKey)? exchangeAndRoutingKey = null)
            : base(settings, logger, queueName, exchangeAndRoutingKey)
        {
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Создаем consumer для обработки сообщений
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Deserialize?.Invoke(body);
                if (message != null)
                {
                    if (DoAsync == null)
                    {
                        return;
                    }

                    //await DoAsync.Invoke(message);
                    var res = DoAsync.Invoke(message).Result;
                    if (res != null)
                    {
                        _logger.LogInformation("Result: {res}", res);
                    }
                }
            };

            // Начинаем слушать очередь
            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);

            // Ожидаем завершения работы сервиса
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
