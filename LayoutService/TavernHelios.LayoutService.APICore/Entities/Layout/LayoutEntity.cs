using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.LayoutService.Common.Entities;

namespace TavernHelios.LayoutService.APICore.Entities.Layout
{
    public class LayoutEntity : IEntity
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string? RestaurantId { get; set; } = null;

        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;

        public string? ImageStr { get; set; } = null;

        public List<TableEntity> Tables { get; set; } = new List<TableEntity>();
    }
}
