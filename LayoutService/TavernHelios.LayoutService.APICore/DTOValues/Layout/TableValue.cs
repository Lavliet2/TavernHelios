using System.ComponentModel;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    /// <summary>
    /// DTO для представления стола и его мест на схеме
    /// </summary>
    [Description("Данные о столе на схеме")]
    public class TableValue
    {
        /// <summary>
        /// Наименование стола
        /// </summary>
        [Description("Наименование стола")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание стола (например, вместимость или особенности)
        /// </summary>
        [Description("Описание стола (например, вместимость или особенности)")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Список мест, принадлежащих этому столу
        /// </summary>
        [Description("Список мест за столом")]
        public List<SeatValue> Seats { get; set; } = new();

        /// <summary>
        /// Первая точка контура стола
        /// </summary>
        [Description("Первая точка контура стола")]
        public PointValue P1 { get; set; } = new PointValue();

        /// <summary>
        /// Вторая точка контура стола
        /// </summary>
        [Description("Вторая точка контура стола")]
        public PointValue P2 { get; set; } = new PointValue();

        /// <summary>
        /// Третья точка контура стола
        /// </summary>
        [Description("Третья точка контура стола")]
        public PointValue P3 { get; set; } = new PointValue();

        /// <summary>
        /// Четвёртая точка контура стола
        /// </summary>
        [Description("Четвёртая точка контура стола")]
        public PointValue P4 { get; set; } = new PointValue();
    }
}