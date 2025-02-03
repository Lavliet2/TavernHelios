using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TavernHelios.MenuService.ApiCore.Enums;

namespace TavernHelios.MenuService.ApiCore.DTOValues.Menu
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
