using System.Text.Json.Serialization;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.Common.DTOValues;

namespace TavernHelios.Server.Services.ReservedLayoutService
{
    public class ReservedLayoutValue : BaseDtoValue
    {
        [JsonConstructor]
        public ReservedLayoutValue() 
        {

        }

        public ReservedLayoutValue(LayoutValue layout)
        {
            Id = layout.Id;
            RestaurantId = layout.Id;
            Width = layout.Width;
            Height = layout.Height;
            ImageStr = layout.ImageStr;
            foreach (var table in layout.Tables)
            {
                Tables.Add(new ReservedTableValue(table));
            }
        }

        public string RestaurantId { get; set; } = string.Empty;
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public string ImageStr { get; set; } = string.Empty;
        public List<ReservedTableValue> Tables { get; set; } = new List<ReservedTableValue>();

        public DateTime ReservationDateTime { get; set; }
    }
}
