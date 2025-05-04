using TavernHelios.ReservationService.ApiCore.Interfaces;

namespace TavernHelios.ReservationService.APICore.Entities
{
    public class ReservationEntity : IEntity
    {
        public long Id { get; set; } = 0;

        public DateTime Date { get; set; } = DateTime.MinValue;

        public string PersonId { get; set; } = string.Empty;

        public List<DishReservationEntity> DishReservations { get; set; } = new List<DishReservationEntity>();

        public int SeatNumber { get; set; } = -1;

        public string TableName { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public string LayoutId { get; set; } = string.Empty;

    }
}
