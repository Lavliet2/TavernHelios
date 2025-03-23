using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.Entities;

namespace TavernHelios.MenuService.Common.Entities
{
    /// <summary>
    /// Сущность Меню, хранимая в БД
    /// </summary>
    public class MenuEntity : IEntity
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public List<string> Dishes { get; set; } = new List<string>();

        /// <summary>
        /// Флаг "Объект удален"
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
