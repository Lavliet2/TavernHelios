using System.ComponentModel;
using System.Text.Json.Serialization;
using TavernHelios.MenuService.Common.Enums;

namespace TavernHelios.MenuService.Common.DTOValues.Menu
{
    /// <summary>
    /// DTO для представления блюда
    /// </summary>
    public class DishValue : BaseDtoValue
    {
        [JsonConstructor]
        public DishValue() { }

        /// <summary>
        /// Наименование блюда
        /// </summary>
        [Description("Наименование")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание блюда
        /// </summary>
        [Description("Описание")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Тип блюда (например: Суп, Основное и т.п.)
        /// </summary>
        [Description("Тип блюда")]
        public DishType DishType { get; set; } = DishType.Soup;

        /// <summary>
        /// Бинарные данные изображения в формате Base64
        /// </summary>
        [Description("Иллюстрация")]
        public string ImageBase64 { get; set; } = string.Empty;
    }

    /// <summary>
    /// Запрос на фильтрацию блюд
    /// </summary>
    public class DishQueryRequestValue
    {
        /// <summary>
        /// Идентификатор блюда (необязательный)
        /// </summary>
        [Description("Идентификатор блюда")]
        public string? DishId { get; set; } = null;

        /// <summary>
        /// Наименование блюда для фильтрации (необязательное)
        /// </summary>
        [Description("Наименование")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// Описание блюда для фильтрации (необязательное)
        /// </summary>
        [Description("Описание")]
        public string? Description { get; set; } = null;

        /// <summary>
        /// Тип блюда для фильтрации (необязательный)
        /// </summary>
        [Description("Тип блюда")]
        public DishType? DishType { get; set; } = null;

        /// <summary>
        /// Флаг удаления для фильтрации (необязательный)
        /// </summary>
        [Description("Флаг удаления")]
        public bool? IsDeleted { get; set; } = null;
    }
}