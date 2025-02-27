using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.AdministrationService.APICore.Settings;

namespace TavernHelios.AdministrationService.APICore.Helpers
{
    public class ConnectionHelper
    {
        public static string GetRabbitMqConnectionString(RabbitMqSettings settings)
        {
            return $"amqp://{settings.User}:{settings.Password}@{settings.Host}:{settings.Port}/";
        }

        public static string GetClickHouseConnectionString(ClickHouseSettings settings)
        {
            return $"Host={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.User};Password={settings.Password};";
        }
    }
}
