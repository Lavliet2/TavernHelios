using TavernHelios.ReservationService.ApiCore.Interfaces;

namespace TavernHelios.ReservationService.APICore.Entities
{
    public class DishReservationEntity : IEntity
    {
        public long Id { get; set; } = 0;

        public string DishId { get; set; } = string.Empty;

        public long ReservationId { get; set; } // Foreign key to Reservation
        public ReservationEntity Reservation { get; set; } //автозаполняется по foreign key
    }
}
