using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.Entities.Layout
{
    public class TableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<SeatEntity> Seats { get; set; } = new();

        public PointEntity P1 { get; set; } = new PointEntity();
        public PointEntity P2 { get; set; } = new PointEntity();
        public PointEntity P3 { get; set; } = new PointEntity();
        public  PointEntity P4 { get; set; } = new PointEntity();
    }
}
