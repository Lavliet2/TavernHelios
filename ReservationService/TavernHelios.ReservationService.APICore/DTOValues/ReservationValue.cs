namespace TavernHelios.ReservationService.APICore.DTOValues
{
    public class ReservationValue
    {
        public string Id { get; set; } = string.Empty;

        public string PersonId { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.MinValue;

        public ICollection<string> DishIds = new List<string>();
    }
}
