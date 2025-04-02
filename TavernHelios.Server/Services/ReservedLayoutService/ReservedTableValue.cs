using System.Text.Json.Serialization;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;

namespace TavernHelios.Server.Services.ReservedLayoutService
{
    public class ReservedTableValue
    {
        [JsonConstructor]
        public ReservedTableValue() { }

        public ReservedTableValue(TableValue table) 
        {
            Name = table.Name;
            Description = table.Description;
            P1 = table.P1;
            P2 = table.P2;
            P3 = table.P3;
            P4 = table.P4;
            foreach (var seat in table.Seats)
            {
                Seats.Add(new ReservedSeatValue(seat));
            }
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<ReservedSeatValue> Seats { get; set; } = new();

        public PointValue P1 { get; set; } = new PointValue();
        public PointValue P2 { get; set; } = new PointValue();
        public PointValue P3 { get; set; } = new PointValue();
        public PointValue P4 { get; set; } = new PointValue();

        public bool IsReserved { get; set; }
    }
}
