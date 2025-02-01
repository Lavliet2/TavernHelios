using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.ReservationService.ApiCore.Interfaces;

namespace TavernHelios.ReservationService.APICore.Entities
{
    public class ReservationEntity : IEntity
    {
        public long Id { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.MinValue;

        public string PersonId { get; set; } = string.Empty;

        public List<string> DishIds { get; set; }
    }
}
