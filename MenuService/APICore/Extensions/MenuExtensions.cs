using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.APICore.DTOValues.Menu;
using Google.Protobuf.WellKnownTypes;
using System.Linq.Expressions;
using TavernHelios.Common.Extensions;

namespace TavernHelios.MenuService.Common.Extensions
{
    public static class MenuExtensions
    {
        public static MenuValue ToDto(this GrpcContract.MenuService.Menu entity)
        {
            var result = new MenuValue();
            result.Id = entity.Id;
            result.Name = entity.Name;
            result.Dishes = entity.Dishes.ToList();
            result.IsDeleted = entity.IsDeleted;
            return result;
        }

        public static MenuEntity ToEntity(this GrpcContract.MenuService.Menu menuValue)
        {
            var result = new MenuEntity();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes = menuValue.Dishes.ToList();
            result.IsDeleted = menuValue.IsDeleted;
            return result;
        }

        public static GrpcContract.MenuService.Menu ToGrpc(this MenuValue menuValue)
        {
            var result = new GrpcContract.MenuService.Menu();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes.AddRange(menuValue.Dishes);
            result.IsDeleted = menuValue.IsDeleted;
            return result;
        }

        public static GrpcContract.MenuService.Menu ToGrpc(this MenuEntity menuValue)
        {
            var result = new GrpcContract.MenuService.Menu();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes.AddRange(menuValue.Dishes);
            result.IsDeleted = menuValue.IsDeleted;
            return result;
        }

        public static GrpcContract.MenuService.Menu ToGrpc(this MenuCreateValue value)
        {
            var result = new GrpcContract.MenuService.Menu();
            result.Id = "";
            result.Name = value.Name;
            result.Dishes.AddRange(value.Dishes);
            return result;
        }

        public static GrpcContract.MenuService.MenuQueryRequest ToGrpc(this MenuQueryRequestValue value)
        {
            var result = new GrpcContract.MenuService.MenuQueryRequest();

            if (value.MenuId != null) result.MenuId = value.MenuId;
            if (value.IsDeleted != null) result.IsDeleted = value.IsDeleted.Value;
            if (value.Name != null) result.Name = value.Name;
            if (value.DishId != null) result.DishId = value.DishId;

            return result;
        }

        public static Expression<Func<MenuEntity, bool>> ToQuery(
            this GrpcContract.MenuService.MenuQueryRequest value
            )
        {
            Expression<Func<MenuEntity, bool>> result = x => true;

            if (value.HasMenuId)
            {
                result = result.AndAlso(x => x.Id == value.MenuId);
            }

            if (value.HasDishId)
            {
                result = result.AndAlso(x => x.Dishes.Contains(value.DishId));
            }

            if (value.HasIsDeleted)
            {
                result = result.AndAlso(x => x.IsDeleted == value.IsDeleted);
            }

            if (value.HasName)
            {
                result = result.AndAlso(x => x.Name == value.Name);
            }

            return result;
        }

    }
}
