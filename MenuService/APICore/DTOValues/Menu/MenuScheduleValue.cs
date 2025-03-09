using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.DTOValues;
using TavernHelios.MenuService.Common.DTOValues.Menu;

namespace TavernHelios.MenuService.APICore.DTOValues.Menu
{
    public class MenuScheduleReplyValue : BaseDtoValue
    {
        [JsonConstructor]
        public MenuScheduleReplyValue()
        {
        }

        public MenuValue Menu { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class MenuScheduleQueryRequestValue
    {
        public string? ScheduleId { get; set; } = null;
        public string? MenuId { get; set; } = null;

        public DateTime? BeginDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
    }

    public class MenuScheduleCreateValue
    {
        public string MenuId { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class MenuScheduleUpdateValue
    {
        public string Id { get; set; } = string.Empty;

        public string MenuId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
