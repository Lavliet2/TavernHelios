using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.ClickHouse.Entities;
using TavernHelios.RabbitMq.Services;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.Services
{
    public class AdministrationService
    {
        private RabbitMqProducerService<LogEntity> _rabbitMqProducer;
        private LogRepository _logRepository;

        public AdministrationService(
            RabbitMqProducerService<LogEntity> rabbitMqProducer,
            LogRepository logRepository)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _logRepository = logRepository;
        }

        public async void GetLogsAsync()
        {
            var entities = await _logRepository.GetByQueryAsync();
            await _rabbitMqProducer.SendMessages(entities);
        }
    }
}
