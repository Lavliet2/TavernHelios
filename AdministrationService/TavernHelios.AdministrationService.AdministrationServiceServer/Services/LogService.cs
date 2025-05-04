using TavernHelios.AdministrationService.ClickHouse;
using TavernHelios.AdministrationService.ClickHouse.Entities;
using TavernHelios.RabbitMq.Services;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.Services
{
    public class LogService
    {
        private LogRepository _logRepository;

        public LogService(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<LogEntity>> GetLogsAsync()
        {
            return await _logRepository.GetByQueryAsync();
        }
    }
}
