using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TavernHelios.MenuService.Common.DTOValues.Menu
{
    public class MenuEditRequest
    {
        [JsonConstructor]
        public MenuEditRequest() { }
        public string MenuId { get; set; } = string.Empty;

        public string DishId { get; set; } = string.Empty;
    }
}
