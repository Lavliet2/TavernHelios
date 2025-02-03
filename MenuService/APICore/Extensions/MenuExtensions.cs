using MongoRepositories.Entities;
using TavernHelios.MenuService.ApiCore.DTOValues.Menu;

namespace TavernHelios.MenuService.ApiCore.Extensions
{
    public static class MenuExtensions
    {
        public static MenuValue ToDto(this GrpcContract.MenuService.Menu entity)
        {
            var result = new MenuValue();
            result.Id = entity.Id;
            result.Name = entity.Name;
            result.Dishes = entity.Dishes.ToList();
            return result;
        }

        public static MenuEntity ToEntity(this GrpcContract.MenuService.Menu menuValue)
        {
            var result = new MenuEntity();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes = menuValue.Dishes.ToList();
            return result;
        }

        public static GrpcContract.MenuService.Menu ToGrpc(this MenuValue menuValue)
        {
            var result = new GrpcContract.MenuService.Menu();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes.AddRange(menuValue.Dishes);
            return result;
        }

        public static GrpcContract.MenuService.Menu ToGrpc(this MenuEntity menuValue)
        {
            var result = new GrpcContract.MenuService.Menu();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes.AddRange(menuValue.Dishes);
            return result;
        }

    }
}
