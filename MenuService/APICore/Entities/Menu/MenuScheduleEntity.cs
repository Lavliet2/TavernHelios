using TavernHelios.MenuService.Common.Entities;

namespace TavernHelios.MenuService.APICore.Entities.Menu
{
    //Таблица расписаний меню на день
    public class MenuScheduleEntity : IEntity
    {
        public string Id { get; set; } = string.Empty;

        public string MenuId { get; set; } = string.Empty;

        public DateTime DateTime { get; set; } = DateTime.MinValue;
    }
}
