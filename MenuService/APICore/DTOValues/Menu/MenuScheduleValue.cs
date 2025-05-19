using System.ComponentModel;
using System.Text.Json.Serialization;
using TavernHelios.MenuService.Common.DTOValues;
using TavernHelios.MenuService.Common.DTOValues.Menu;

namespace TavernHelios.MenuService.APICore.DTOValues.Menu
{
    /// <summary>
    /// Ответ с данными расписания меню
    /// </summary>
    public class MenuScheduleReplyValue : BaseDtoValue
    {
        [JsonConstructor]
        public MenuScheduleReplyValue()
        {
        }

        /// <summary>
        /// Меню, назначенное по расписанию
        /// </summary>
        [Description("Меню")]
        public MenuValue Menu { get; set; }

        /// <summary>
        /// Дата и время, на которые назначено меню
        /// </summary>
        [Description("Дата")]
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// Запрос для фильтрации записей расписания меню
    /// </summary>
    public class MenuScheduleQueryRequestValue
    {
        /// <summary>
        /// Идентификатор записи расписания (необязательный)
        /// </summary>
        [Description("Идентификатор расписания")]
        public string? ScheduleId { get; set; } = null;

        /// <summary>
        /// Идентификатор меню для фильтрации (необязательный)
        /// </summary>
        [Description("Идентификатор меню")]
        public string? MenuId { get; set; } = null;

        /// <summary>
        /// Начальная дата для фильтрации записей (необязательная)
        /// </summary>
        [Description("Дата начала")]
        public DateTime? BeginDate { get; set; } = null;

        /// <summary>
        /// Конечная дата для фильтрации записей (необязательная)
        /// </summary>
        [Description("Дата окончания")]
        public DateTime? EndDate { get; set; } = null;

        /// <summary>
        /// Флаг: удалена ли запись (необязательный)
        /// </summary>
        [Description("Флаг удаления")]
        public bool? IsDeleted { get; set; } = null;
    }

    /// <summary>
    /// Запрос на создание новой записи расписания меню
    /// </summary>
    public class MenuScheduleCreateValue
    {
        /// <summary>
        /// Идентификатор меню, которое нужно назначить
        /// </summary>
        [Description("Идентификатор")]
        public string MenuId { get; set; }

        /// <summary>
        /// Дата и время, на которые назначается меню
        /// </summary>
        [Description("Дата")]
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// Запрос на обновление существующей записи расписания меню
    /// </summary>
    public class MenuScheduleUpdateValue
    {
        /// <summary>
        /// Уникальный идентификатор записи расписания
        /// </summary>
        [Description("Идентификатор")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Новый идентификатор меню для этой записи
        /// </summary>
        [Description("Идентификатор")]
        public string MenuId { get; set; }

        /// <summary>
        /// Новая дата и время, на которые назначается меню
        /// </summary>
        [Description("Дата")]
        public DateTime DateTime { get; set; }
    }
}