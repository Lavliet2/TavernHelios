using System.ComponentModel;

namespace TavernHelios.ReservationService.APICore.DTOValues
{
    /// <summary>
    /// Данные о бронировании
    /// </summary>
    [Description("Данные о бронировании")]
    public class ReservationValue
    {
        /// <summary>
        /// Уникальный идентификатор бронирования
        /// </summary>
        [Description("Уникальный идентификатор бронирования")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор клиента, сделавшего бронь
        /// </summary>
        [Description("Идентификатор клиента, сделавшего бронь")]
        public string PersonId { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время бронирования
        /// </summary>
        [Description("Дата и время бронирования")]
        public DateTime Date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Список идентификаторов блюд, заказанных при бронировании
        /// </summary>
        [Description("Список идентификаторов блюд, заказанных при бронировании")]
        public List<string> DishIds { get; set; } = new List<string>();

        /// <summary>
        /// Номер места, на которое сделана запись
        /// </summary>
        [Description("Номер места, на которое сделана запись")]
        public int SeatNumber { get; set; } = -1;

        /// <summary>
        /// Наименование стола, за который забронировано место
        /// </summary>
        [Description("Наименование стола, за который забронировано место")]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Флаг: удалено ли бронирование
        /// </summary>
        [Description("Флаг: удалено ли бронирование")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Идентификатор схемы зала, к которой относится бронирование
        /// </summary>
        [Description("Идентификатор схемы зала, к которой относится бронирование")]
        public string LayoutId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Запрос на создание бронирования
    /// </summary>
    [Description("Запрос на создание бронирования")]
    public class ReservationCreateValue
    {
        /// <summary>
        /// Идентификатор клиента, делающего бронь
        /// </summary>
        [Description("Идентификатор клиента, делающего бронь")]
        public string PersonId { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время бронирования
        /// </summary>
        [Description("Дата и время бронирования")]
        public DateTime Date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Список идентификаторов блюд, заказываемых при бронировании
        /// </summary>
        [Description("Список идентификаторов блюд, заказываемых при бронировании")]
        public List<string> DishIds { get; set; } = new List<string>();

        /// <summary>
        /// Номер места, на которое делается запись
        /// </summary>
        [Description("Номер места, на которое делается запись")]
        public int SeatNumber { get; set; } = -1;

        /// <summary>
        /// Наименование стола, за который забронировано место
        /// </summary>
        [Description("Наименование стола, за который забронировано место")]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор схемы зала, к которой относится бронирование
        /// </summary>
        [Description("Идентификатор схемы зала, к которой относится бронирование")]
        public string LayoutId { get; set; } = string.Empty;
    }
}