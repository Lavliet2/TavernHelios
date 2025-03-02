using Microsoft.Extensions.Options;
using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.ClickHouse.Converters;
using TavernHelios.AdministrationService.ClickHouse.Entities;
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

        public override void Dispose()
        {
            base.Dispose();

            Deserialize -= ToLogEntityConverter.ToLogEntity;
            DoAsync -= _logRepository.CreateAsync;
        }
    }
}
