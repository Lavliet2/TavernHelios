using Microsoft.Extensions.Options;
using TavernHelios.AdministrationService.APICore.Converters;
using TavernHelios.AdministrationService.APICore.Entities;
using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.RabbitMq.Services;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.AdministrationService.RabbitMqConsumer.Services
{
    public class LogConsumerService : RabbitMqConsumerService<LogEntity>
    {
        private readonly LogRepository _logRepository;

        public LogConsumerService(
            IOptions<RabbitMqSettings> settings, 
            LogRepository logRepository)
            : base(settings, "logs", ("logs_exchange", "log"))
        {
            _logRepository = logRepository;

            Deserialize += ToLogEntityConverter.ToLogEntity;
            DoAsync += _logRepository.CreateAsync;
        }

        //public async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    // Создаем consumer для обработки сообщений
        //    var consumer = new AsyncEventingBasicConsumer(_channel);
        //    consumer.ReceivedAsync += async (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        var log = JsonConverter.ToLogEntity(message);

        //        // Сохраняем лог в ClickHouse
        //        if (log != null)
        //        {
        //            await _logRepository.CreateAsync(log);
        //        }
        //    };

        //    // Начинаем слушать очередь
        //    await _channel.BasicConsumeAsync(
        //        queue: "logs",
        //        autoAck: true,
        //        consumer: consumer);

        //    // Ожидаем завершения работы сервиса
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}

        public void Dispose()
        {
            base.Dispose();

            Deserialize -= ToLogEntityConverter.ToLogEntity;
            DoAsync -= _logRepository.CreateAsync;
        }
    }
}
