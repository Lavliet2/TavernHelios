using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.Entities.Layout
{
    public struct PointEntity
    {
        [JsonConstructor]
        public PointEntity()
        {

        }
        
        public PointEntity(double x, double y)
        {
            X = x; 
            Y = y;
        }

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
    }
}
