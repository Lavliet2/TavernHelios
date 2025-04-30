using TavernHelios.AdministrationService.RabbitMqConsumer.Services;

namespace TavernHelios.AdministrationService.AdministrationServiceServer.BackgroundServices
{
    public class RabbitMqBackgroundService : BackgroundService
    {
        private readonly LogConsumerService _logConsumerService;

        public RabbitMqBackgroundService(LogConsumerService logConsumerService)
        {
            _logConsumerService = logConsumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _logConsumerService.ExecuteAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _logConsumerService.Dispose();
            base.Dispose();
        }
    }
}
