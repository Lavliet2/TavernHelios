using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    public struct PointValue
    {
        [JsonConstructor]
        public PointValue()
        {

        }
        
        public PointValue(double x, double y)
        {
            X = x; 
            Y = y;
        }

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
    }
}
