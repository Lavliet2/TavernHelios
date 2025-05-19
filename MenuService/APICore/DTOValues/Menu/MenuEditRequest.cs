using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TavernHelios.MenuService.Common.DTOValues.Menu
{
    /// <summary>
    /// DTO для запроса на редактирование меню (например, добавление/удаление блюда)
    /// </summary>
    public class MenuEditRequest
    {
        [JsonConstructor]
        public MenuEditRequest() { }

        /// <summary>
        /// Идентификатор меню, которое нужно изменить
        /// </summary>
        [Description("Идентификатор меню")]
        public string MenuId { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор блюда, которое добавляется или удаляется из меню
        /// </summary>
        [Description("Идентификатор блюда")]
        public string DishId { get; set; } = string.Empty;
    }
}