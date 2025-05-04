using System.Text.Json.Serialization;
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

    public class DishQueryRequestValue
    {
        public string? DishId { get; set; } = null;
        public string? Name{ get; set; } = null;
        public string? Description { get; set; } = null;

        public DishType? DishType { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
    }
}
