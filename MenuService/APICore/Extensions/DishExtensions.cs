using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Enums;

namespace TavernHelios.MenuService.Common.Extensions
{
    public static class DishExtensions
    {
        public static DishValue ToDto(this GrpcContract.MenuService.Dish entity)
        {
            var result = new DishValue();
            result.Id = entity.Id;
            result.Description = entity.Description;
            result.Name = entity.Name;
            result.ImageBase64 = entity.ImageBase64;
            result.DishType = (DishType)entity.DishType;
            return result;
        }

        public static DishEntity ToEntity(this GrpcContract.MenuService.Dish dishValue)
        {
            var result = new DishEntity();
            result.Id = dishValue.Id;
            result.Name= dishValue.Name;
            result.DishType = (DishType)dishValue.DishType;
            result.Description = dishValue.Description;
            result.ImageBase64 = dishValue.ImageBase64;
            return result;
        }

        public static GrpcContract.MenuService.Dish ToGrpc(this DishValue dishValue)
        {
            var result = new GrpcContract.MenuService.Dish();
            result.Id = dishValue.Id;
            result.Name = dishValue.Name;
            result.DishType = (int)dishValue.DishType;
            result.Description = dishValue.Description;
            result.ImageBase64 = dishValue.ImageBase64;
            return result;
        }

        public static GrpcContract.MenuService.Dish ToGrpc(this DishEntity dishValue)
        {
            var result = new GrpcContract.MenuService.Dish();
            result.Id = dishValue.Id;
            result.Name = dishValue.Name;
            result.DishType = (int)dishValue.DishType;
            result.Description = dishValue.Description;
            result.ImageBase64 = dishValue.ImageBase64;
            return result;
        }

    }
}
