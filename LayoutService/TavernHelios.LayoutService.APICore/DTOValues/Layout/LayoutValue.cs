using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcContract.LayoutService;
using TavernHelios.LayoutService.Common.DTOValues;

namespace TavernHelios.LayoutService.APICore.DTOValues.Layout
{
    public class LayoutValue : BaseDtoValue
    {
        public string RestaurantId { get; set; }
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public string ImageStr { get; set; } = string.Empty;
        public List<TableValue> Tables { get; set; } = new List<TableValue>();
        public List<SeatValue> Seats { get; set; } = new List<SeatValue>();
    }

    public class AddLayoutRequestValue
    {
        public string RestaurantId { get; set; }
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;

        public string ImageStr { get; set; } = string.Empty;
        public List<TableValue> Tables { get; set; } = new List<TableValue>();
        public List<SeatValue> Seats { get; set; } = new List<SeatValue>();
    }

    public class LayoutQueryRequestValue
    {
        public string? LayoutId { get; set; } = null;
        public string? RestaurantId { get; set; } = null;

        public bool? IsDeleted { get; set; } = null;
    }
}
