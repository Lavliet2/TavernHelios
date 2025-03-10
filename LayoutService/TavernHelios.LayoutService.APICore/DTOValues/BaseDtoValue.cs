using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.Common.DTOValues
{
    public class BaseDtoValue
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Флаг "Объект удален"
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
