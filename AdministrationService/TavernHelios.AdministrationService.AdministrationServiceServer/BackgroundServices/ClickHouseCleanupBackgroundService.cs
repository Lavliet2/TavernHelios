using WebAdmin.BackgroundServices;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.BackgroundServices
{
    public class ClickHouseCleanupBackgroundService : BackgroundService
    {
        private readonly ClickHouseCleanupService _clickHouseCleanupService;

        public ClickHouseCleanupBackgroundService(ClickHouseCleanupService clickHouseCleanupService)
        {
            _clickHouseCleanupService = clickHouseCleanupService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _clickHouseCleanupService.ExecuteAsync(stoppingToken);
        }
    }
}
