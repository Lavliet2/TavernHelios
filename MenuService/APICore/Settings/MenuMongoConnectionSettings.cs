﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.MenuService.Common.Settings
{
    public class MenuMongoConnectionSettings
    {
        public string Ip { get; set; } = "";
        public int Port { get; set; }

        public string DbName { get; set; } = "";

        public bool ReInitDbWithTestData { get; set; } = false;

        public string GetConnectionString()
        {
            return $"mongodb://{Ip}:{Port}";
        }

    }
}
