using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.Common.Extensions;
using TavernHelios.LayoutService.APICore.DTOValues.Layout;
using TavernHelios.LayoutService.APICore.Entities.Layout;

namespace TavernHelios.LayoutService.APICore.Extensions
{
    public static class LayoutExtensions
    {
        public static LayoutValue ToDto(this GrpcContract.LayoutService.Layout entity)
        {
            var result = new LayoutValue();
            result.Id = entity.Id;
            result.IsDeleted = entity.IsDeleted;
            result.RestaurantId = entity.RestaurantId;
            result.Width = entity.Width;
            result.Height = entity.Height;
            result.ImageStr = entity.ImageStr ?? string.Empty;
            result.Seats = new List<SeatValue>(entity.Seats.Select(x => x.ToDto()));
            result.Tables = new List<TableValue>(entity.Tables.Select(x => x.ToDto()));
            return result;
        }

        public static LayoutEntity ToEntity(this GrpcContract.LayoutService.Layout layoutValue)
        {
            var result = new LayoutEntity();
            result.Id = layoutValue.Id;
            result.IsDeleted = layoutValue.IsDeleted;
            result.RestaurantId = layoutValue.RestaurantId;
            result.Width = layoutValue.Width;
            result.Height = layoutValue.Height;
            result.ImageStr = layoutValue.ImageStr;
            result.Seats = new List<SeatEntity>(layoutValue.Seats.Select(x => x.ToEntity()));
            result.Tables = new List<TableEntity>(layoutValue.Tables.Select(x => x.ToEntity()));
            return result;
        }

        public static GrpcContract.LayoutService.Layout ToGrpc(this LayoutValue layoutValue)
        {
            var result = new GrpcContract.LayoutService.Layout();
            result.Id = layoutValue.Id;
            result.IsDeleted = layoutValue.IsDeleted;
            result.RestaurantId = layoutValue.RestaurantId;
            result.Width = layoutValue.Width;
            result.Height = layoutValue.Height;
            result.ImageStr = layoutValue.ImageStr;
            result.Seats.AddRange(layoutValue.Seats.Select(x => x.ToGrpc()));
            result.Tables.AddRange(layoutValue.Tables.Select(x => x.ToGrpc()));

            return result;
        }

        public static GrpcContract.LayoutService.Layout ToGrpc(this LayoutEntity layoutValue)
        {
            var result = new GrpcContract.LayoutService.Layout();
            result.Id = layoutValue.Id;
            result.IsDeleted = layoutValue.IsDeleted;
            result.RestaurantId = layoutValue.RestaurantId;
            result.Width = layoutValue.Width;
            result.Height = layoutValue.Height;
            result.ImageStr = layoutValue.ImageStr;
            result.Seats.AddRange(layoutValue.Seats.Select(x => x.ToGrpc()));
            result.Tables.AddRange(layoutValue.Tables.Select(x => x.ToGrpc()));
            return result;
        }

        public static LayoutEntity ToEntity(this GrpcContract.LayoutService.AddLayoutRequest layoutValue)
        {
            var result = new LayoutEntity();
            result.Id = null;
            result.RestaurantId = layoutValue.RestaurantId;
            result.Width = layoutValue.Width;
            result.Height = layoutValue.Height;
            result.ImageStr = layoutValue.ImageStr;
            result.Seats = new List<SeatEntity>(layoutValue.Seats.Select(x => x.ToEntity()));
            result.Tables = new List<TableEntity>(layoutValue.Tables.Select(x => x.ToEntity()));
            return result;
        }

        public static GrpcContract.LayoutService.AddLayoutRequest ToGrpc(this AddLayoutRequestValue layoutValue)
        {
            var result = new GrpcContract.LayoutService.AddLayoutRequest();
            result.RestaurantId = layoutValue.RestaurantId;
            result.Width = layoutValue.Width;
            result.Height = layoutValue.Height;
            result.ImageStr = layoutValue.ImageStr;
            result.Seats.AddRange(layoutValue.Seats.Select(x => x.ToGrpc()));
            result.Tables.AddRange(layoutValue.Tables.Select(x => x.ToGrpc()));
            return result;
        }

        public static GrpcContract.LayoutService.LayoutQueryRequest ToGrpc(this LayoutQueryRequestValue value)
        {
            var result = new GrpcContract.LayoutService.LayoutQueryRequest();

            if (value.LayoutId != null) result.LayoutId = value.LayoutId;
            if (value.RestaurantId != null) result.RestaurantId = value.RestaurantId;
            if (value.IsDeleted != null) result.IsDeleted = value.IsDeleted.Value;
            return result;
        }

        public static Expression<Func<LayoutEntity, bool>> ToQuery(
            this GrpcContract.LayoutService.LayoutQueryRequest value
            )
        {
            Expression<Func<LayoutEntity, bool>> result = x => true;

            if (value.HasLayoutId)
            {
                result = result.AndAlso(x => x.Id == value.LayoutId);
            }

            if (value.HasIsDeleted)
            {
                result = result.AndAlso(x => x.IsDeleted == value.IsDeleted);
            }

            if (value.HasRestaurantId)
            {
                result = result.AndAlso(x => x.RestaurantId == value.RestaurantId);
            }

            return result;
        }
    }
}
