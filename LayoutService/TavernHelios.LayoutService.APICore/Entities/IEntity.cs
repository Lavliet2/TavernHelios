using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.Common.Entities
{
    public interface IEntity
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Флаг "Объект удален"
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
