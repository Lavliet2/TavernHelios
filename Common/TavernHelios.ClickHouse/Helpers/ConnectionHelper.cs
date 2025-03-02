﻿using TavernHelios.ClickHouse.Settings;

namespace TavernHelios.ClickHouse.Helpers
{
    public class ConnectionHelper
    {
        public static string GetClickHouseConnectionString(ClickHouseSettings settings)
        {
            return $"Host={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.User};Password={settings.Password};";
        }
    }
}
