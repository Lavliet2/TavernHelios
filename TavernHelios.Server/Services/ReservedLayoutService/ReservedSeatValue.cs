using System.Text.Json.Serialization;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;

namespace TavernHelios.Server.Services.ReservedLayoutService
{
    public class ReservedSeatValue
    {
        [JsonConstructor]
        public ReservedSeatValue() { }

        public ReservedSeatValue(SeatValue seat) 
        {
            Number = seat.Number;
            Description = seat.Description;
            Center = seat.Center;
            Radius = seat.Radius;
        }

        public int Number { get; set; } = 0;
        public string Description { get; set; } = string.Empty;

        public PointValue Center { get; set; } = new PointValue();
        public double Radius { get; set; } = 0;

        public bool IsReserved { get; set; } = false;
    }
}
