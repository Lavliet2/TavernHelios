using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    public class SeatValue
    {
        public int Number { get; set; } = 0;
        public string Description { get; set; } = string.Empty;

        public PointValue Center { get; set; } = new PointValue();
        public double Radius { get; set; } = 0;
    }
}
