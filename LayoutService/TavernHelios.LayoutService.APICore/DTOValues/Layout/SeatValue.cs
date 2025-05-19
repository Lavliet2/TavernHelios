using System.ComponentModel;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    /// <summary>
    /// DTO для представления одного места (сиденья) за столом
    /// </summary>
    public class SeatValue
    {
        /// <summary>
        /// Номер места (например, порядковый номер)
        /// </summary>
        [Description("Номер места")]
        public int Number { get; set; } = 0;

        /// <summary>
        /// Описание места (например, особенности или примечания)
        /// </summary>
        [Description("Описание места")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Центральная точка места (координаты)
        /// </summary>
        [Description("Координаты центра")]
        public PointValue Center { get; set; } = new PointValue();

        /// <summary>
        /// Радиус действия места (в условных единицах)
        /// </summary>
        [Description("Радиус")]
        public double Radius { get; set; } = 0;
    }
}