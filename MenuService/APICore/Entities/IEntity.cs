using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.MenuService.Common.Entities
{
    public interface IEntity
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        string Id { get; set; }
    }
}
