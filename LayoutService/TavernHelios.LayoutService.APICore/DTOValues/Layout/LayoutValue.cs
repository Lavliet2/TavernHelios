using System.ComponentModel;
using TavernHelios.LayoutService.Common.DTOValues;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    /// <summary>
    /// Данные о схеме расположения зала ресторана
    /// </summary>
    [Description("Данные о схеме расположения зала")]
    public class LayoutValue : BaseDtoValue
    {
        /// <summary>
        /// Идентификатор ресторана, к которому относится данная схема
        /// </summary>
        [Description("Идентификатор ресторана")]
        public string RestaurantId { get; set; } = string.Empty;

        /// <summary>
        /// Ширина схемы в условных единицах
        /// </summary>
        [Description("Ширина схемы")]
        public double Width { get; set; } = 0;

        /// <summary>
        /// Высота схемы в условных единицах
        /// </summary>
        [Description("Высота схемы")]
        public double Height { get; set; } = 0;

        /// <summary>
        /// Строковое представление изображения схемы (например, Base64)
        /// </summary>
        [Description("Строковое представление изображения схемы")]
        public string ImageStr { get; set; } = string.Empty;

        /// <summary>
        /// Список столов, расположенных на данной схеме
        /// </summary>
        [Description("Список столов на схеме")]
        public List<TableValue> Tables { get; set; } = new List<TableValue>();
    }

    /// <summary>
    /// Запрос на создание новой схемы расположения
    /// </summary>
    [Description("Запрос на добавление новой схемы")]
    public class AddLayoutRequestValue
    {
        /// <summary>
        /// Идентификатор ресторана, для которого создаётся схема
        /// </summary>
        [Description("Идентификатор ресторана")]
        public string RestaurantId { get; set; }

        /// <summary>
        /// Ширина схемы в условных единицах
        /// </summary>
        [Description("Ширина схемы")]
        public double Width { get; set; } = 0;

        /// <summary>
        /// Высота схемы в условных единицах
        /// </summary>
        [Description("Высота схемы")]
        public double Height { get; set; } = 0;

        /// <summary>
        /// Строковое представление изображения схемы (например, Base64)
        /// </summary>
        [Description("Строковое представление изображения схемы")]
        public string ImageStr { get; set; } = string.Empty;

        /// <summary>
        /// Список столов, которые нужно добавить на схему
        /// </summary>
        [Description("Список столов для добавления на схему")]
        public List<TableValue> Tables { get; set; } = new List<TableValue>();
    }

    /// <summary>
    /// Параметры запроса для фильтрации схем
    /// </summary>
    [Description("Параметры запроса для фильтрации схем")]
    public class LayoutQueryRequestValue
    {
        /// <summary>
        /// Идентификатор схемы (необязательный)
        /// </summary>
        [Description("Идентификатор схемы (необязательный)")]
        public string? LayoutId { get; set; } = null;

        /// <summary>
        /// Идентификатор ресторана (необязательный)
        /// </summary>
        [Description("Идентификатор ресторана (необязательный)")]
        public string? RestaurantId { get; set; } = null;

        /// <summary>
        /// Флаг: удалена ли схема (необязательный)
        /// </summary>
        [Description("Флаг удалена ли схема (необязательный)")]
        public bool? IsDeleted { get; set; } = null;
    }
}