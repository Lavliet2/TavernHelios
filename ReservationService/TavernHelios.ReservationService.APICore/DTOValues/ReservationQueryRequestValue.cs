using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.ReservationService.APICore.DTOValues
{
    [Serializable]
    [DisplayName("Запрос получение одной или нескольких записей о бронировании")]
    public class ReservationQueryRequestValue
    {
        [DisplayName("Id брони")]
        public long? ReservationId { get; set; } = null;
        public string? PersonId { get; set; } = null;
        public string? DishId { get; set; } = null;

        public bool? IsDeleted { get; set; } = null;

        /// <summary>
        /// Нижняя граница времени запроса
        /// </summary>
        public DateTime? BeginDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;

        public string? RestaurantId { get; set; } = null;
    }
}
