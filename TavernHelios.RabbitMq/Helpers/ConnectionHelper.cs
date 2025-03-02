using TavernHelios.RabbitMq.Settings;

namespace TavernHelios.RabbitMq.Helpers
{
    public class ConnectionHelper
    {
        public static string GetRabbitMqConnectionString(RabbitMqSettings settings)
        {
            return $"amqp://{settings.User}:{settings.Password}@{settings.Host}:{settings.Port}/";
        }
    }
}
