using System.ComponentModel;

namespace TavernHelios.ReservationService.APICore.DTOValues
{
    /// <summary>
    /// Параметры запроса для фильтрации записей о бронировании
    /// </summary>
    [Serializable]
    [DisplayName("Запрос получения одной или нескольких записей о бронировании")]
    [Description("Параметры запроса для фильтрации записей о бронировании")]
    public class ReservationQueryRequestValue
    {
        /// <summary>
        /// Уникальный идентификатор записи о бронировании (необязательный)
        /// </summary>
        [DisplayName("Id бронирования")]
        [Description("Уникальный идентификатор записи о бронировании (необязательный)")]
        public long? ReservationId { get; set; } = null;

        /// <summary>
        /// Идентификатор клиента (необязательный)
        /// </summary>
        [Description("Идентификатор клиента (необязательный)")]
        public string? PersonId { get; set; } = null;

        /// <summary>
        /// Идентификатор блюда (необязательный)
        /// </summary>
        [Description("Идентификатор блюда (необязательный)")]
        public string? DishId { get; set; } = null;

        /// <summary>
        /// Флаг: удалена ли запись (необязательный)
        /// </summary>
        [Description("Флаг: удалена ли запись (необязательный)")]
        public bool? IsDeleted { get; set; } = null;

        /// <summary>
        /// Нижняя граница времени запроса
        /// </summary>
        [Description("Начальная дата/время для фильтрации записей")]
        public DateTime? BeginDate { get; set; } = null;

        /// <summary>
        /// Конечная дата/время для фильтрации записей
        /// </summary>
        [Description("Конечная дата/время для фильтрации записей")]
        public DateTime? EndDate { get; set; } = null;

        /// <summary>
        /// Идентификатор схемы зала (необязательный)
        /// </summary>
        [Description("Идентификатор схемы зала (необязательный)")]
        public string? LayoutId { get; set; } = null;
    }
}