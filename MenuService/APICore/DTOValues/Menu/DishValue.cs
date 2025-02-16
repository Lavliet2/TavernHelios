using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.Enums;

namespace TavernHelios.MenuService.Common.DTOValues.Menu
{
    public class DishValue : BaseDtoValue
    {
        [JsonConstructor]
        public DishValue() { }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Тип блюда
        /// </summary>

        public DishType DishType { get; set; } = DishType.Soup;

        /// <summary>
        /// Иллюстрация
        /// </summary>
        public string ImageBase64 { get; set; } = string.Empty;
    }
}
