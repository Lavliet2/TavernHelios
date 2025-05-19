using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TavernHelios.MenuService.Common.DTOValues.Menu
{
    /// <summary>
    /// DTO для представления базовой информации о меню
    /// </summary>
    public class MenuValue : BaseDtoValue
    {
        [JsonConstructor]
        public MenuValue()
        {
        }

        /// <summary>
        /// Наименование меню
        /// </summary>
        [Description("Наименование")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Список идентификаторов блюд, входящих в меню
        /// </summary>
        [Description("Блюда")]
        public List<string> Dishes { get; set; } = new List<string>();
    }

    /// <summary>
    /// DTO для представления расширенной информации о меню (с полными данными о блюдах)
    /// </summary>
    public class MenuValueFull : MenuValue
    {
        [JsonConstructor]
        public MenuValueFull()
        {
        }

        /// <summary>
        /// Список полных данных по блюдам, входящим в меню
        /// </summary>
        [Description("Блюда")]
        public List<DishValue> DishesFull { get; set; } = new List<DishValue>();
    }

    /// <summary>
    /// Запрос на создание нового меню
    /// </summary>
    public class MenuCreateValue
    {
        /// <summary>
        /// Наименование нового меню
        /// </summary>
        [Description("Наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Список идентификаторов блюд, которые будут включены в меню
        /// </summary>
        [Description("Блюда")]
        public List<string> Dishes { get; set; } = new List<string>();
    }

    /// <summary>
    /// Параметры запроса для фильтрации записей о меню
    /// </summary>
    public class MenuQueryRequestValue
    {
        /// <summary>
        /// Идентификатор меню (необязательный)
        /// </summary>
        [Description("Идентификатор меню")]
        public string? MenuId { get; set; } = null;

        /// <summary>
        /// Наименование меню (необязательное)
        /// </summary>
        [Description("Наименование")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// Идентификатор блюда для фильтрации (необязательный)
        /// </summary>
        [Description("Идентификатор блюда")]
        public string? DishId { get; set; } = null;

        /// <summary>
        /// Флаг: удалено ли меню (необязательный)
        /// </summary>
        [Description("Флаг удаления")]
        public bool? IsDeleted { get; set; } = null;
    }
}