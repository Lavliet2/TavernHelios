namespace TavernHelios.ReservationService.APICore.DTOValues
{
    public class ReservationValue
    {
        public string Id { get; set; } = string.Empty;

        public string PersonId { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.MinValue;

        public List<string> DishIds { get; set; } = new List<string>();
    }

    public class ReservationCreateValue
    {
        public string PersonId { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.MinValue;

        public List<string> DishIds { get; set; } = new List<string>();
    }
}
