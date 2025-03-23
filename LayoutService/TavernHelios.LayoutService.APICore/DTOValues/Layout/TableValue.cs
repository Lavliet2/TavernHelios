using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    public class TableValue
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<SeatValue> Seats { get; set; } = new();

        public PointValue P1 { get; set; } = new PointValue();
        public PointValue P2 { get; set; } = new PointValue();
        public PointValue P3 { get; set; } = new PointValue();
        public PointValue P4 { get; set; } = new PointValue();
    }
}
