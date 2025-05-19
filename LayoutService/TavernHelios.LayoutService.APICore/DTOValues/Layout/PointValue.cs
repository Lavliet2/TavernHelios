using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    /// <summary>
    /// Координаты точки (X, Y) на схеме
    /// </summary>
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

        /// <summary>
        /// Координата X точки
        /// </summary>
        [Description("Координата X")]
        public double X { get; set; } = 0;

        /// <summary>
        /// Координата Y точки
        /// </summary>
        [Description("Координата Y")]
        public double Y { get; set; } = 0;
    }
}