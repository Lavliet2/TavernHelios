using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TavernHelios.AdministrationService.APICore.Converters;
using TavernHelios.AdministrationService.APICore.Helpers;
using TavernHelios.AdministrationService.APICore.Settings;
using TavernHelios.AdministrationService.ClickHouse;

namespace TavernHelios.AdministrationService.RabbitMqConsumer.Services
{
    public class LogConsumerService : IDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly LogRepository _logRepository;
        private readonly string _rabbitMqConnectionString;

        public LogConsumerService(IOptions<RabbitMqSettings> settings, LogRepository logRepository)
        {
            _rabbitMqConnectionString = ConnectionHelper.GetRabbitMqConnectionString(settings.Value);
            _logRepository = logRepository;

            InitializeAsync().Wait();
        }


        private async Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqConnectionString) };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Объявляем обмен
            await _channel.ExchangeDeclareAsync(
                exchange: "logs_exchange",
                type: ExchangeType.Direct,
                durable: false,
                autoDelete: false,
                arguments: null);

            // Объявляем очередь
            await _channel.QueueDeclareAsync(
                queue: "logs",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Привязываем очередь к обмену с указанием routing_key
            await _channel.QueueBindAsync(
                queue: "logs",
                exchange: "logs_exchange",
                routingKey: "log"); // Ключ маршрутизации
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Создаем consumer для обработки сообщений
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonConverter.FromJsonToLogEntity(message);

                // Сохраняем лог в ClickHouse
                if (log != null)
                {
                    await _logRepository.CreateAsync(log);
                }
            };

            // Начинаем слушать очередь
            await _channel.BasicConsumeAsync(
                queue: "logs",
                autoAck: true,
                consumer: consumer);

            // Ожидаем завершения работы сервиса
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
        }
    }
}
