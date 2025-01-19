using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Settings
{
    public class MongoConnectionSettings
    {
        public string Ip { get; set; } = "";
        public int Port { get; set; }

        public string DbName { get; set; }

        public string GetConnectionString()
        {
            return $"mongodb://{Ip}:{Port}";
        }
    }
}
