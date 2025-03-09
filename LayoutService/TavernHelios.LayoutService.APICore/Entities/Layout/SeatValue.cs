using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.Entities.Layout
{
    public class SeatEntity
    {
        public int Number { get; set; } = 0;
        public string Desctiption { get; set; } = string.Empty;

        public PointEntity Center { get; set; } = new PointEntity();
        public PointEntity Alignment { get; set; } = new PointEntity();
    }
}
