using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.GrpcCommon.Settings
{
    public class GrpcSettingsBase
    {
        /// <summary>
        /// Прослушиваемый IP-адрес
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Прослушиваемый порт
        /// </summary>
        public int Port { get; set; }
    }
}
