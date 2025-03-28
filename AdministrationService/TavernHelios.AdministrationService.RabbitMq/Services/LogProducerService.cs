using MessagePack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TavernHelios.AdministrationService.ClickHouse.Entities;
using TavernHelios.RabbitMq.Services;
using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.AdministrationService.RabbitMq.Services
{
    public class LogProducerService : RabbitMqProducerService<LogEntity>
    {
        public LogProducerService(
            IOptions<RabbitMqSettings> settings,
            ILogger<LogProducerService> logger)
            : base(settings, logger, "admin_logs")
        {
            SerializeMessage += SerializeMsgPack;
            SerializeMessages += SerializeMsgPack;
        }

        private byte[] SerializeMsgPack(LogEntity logEntity)
        {
            return MessagePackSerializer.Serialize(logEntity);
        }

        private byte[] SerializeMsgPack(IEnumerable<LogEntity> logEntities)
        {
            return MessagePackSerializer.Serialize(logEntities);
        }

        public override void Dispose()
        {
            base.Dispose();

            SerializeMessage -= SerializeMsgPack;
            SerializeMessages -= SerializeMsgPack;
        }
    }
}
