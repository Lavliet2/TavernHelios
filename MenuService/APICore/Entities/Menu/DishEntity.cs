using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Enums;


namespace TavernHelios.MenuService.Common.Entities
{
    /// <summary>
    /// Сущность Блюда, хранимая в БД
    /// </summary>
    public class DishEntity : IEntity
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Тип блюда
        /// </summary>

        public DishType DishType { get; set; } = DishType.Soup;

        /// <summary>
        /// Иллюстрация
        /// </summary>
        public string ImageBase64 { get; set; } = String.Empty;

        /// <summary>
        /// Флаг "Объект удален"
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
