using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.ClickHouse.Entities;
using TavernHelios.RabbitMq.Services;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.Services
{
    public class LogService
    {
        //private RabbitMqProducerService<LogEntity> _rabbitMqProducer;
        private LogRepository _logRepository;

        public LogService(
            //RabbitMqProducerService<LogEntity> rabbitMqProducer,
            LogRepository logRepository)
        {
            //_rabbitMqProducer = rabbitMqProducer;
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<LogEntity>> GetLogsAsync()
        {
            return await _logRepository.GetByQueryAsync();
            //await _rabbitMqProducer.SendMessages(entities);
        }
    }
}
