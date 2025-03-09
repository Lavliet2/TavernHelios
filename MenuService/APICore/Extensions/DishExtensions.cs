using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using TavernHelios.Common.Extensions;
using TavernHelios.MenuService.APICore.Entities.Menu;
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
            result.IsDeleted = entity.IsDeleted;
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
            result.IsDeleted = dishValue.IsDeleted;
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
            result.IsDeleted = dishValue.IsDeleted;
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
            result.IsDeleted = dishValue.IsDeleted;
            return result;
        }

        public static GrpcContract.MenuService.DishQueryRequest ToGrpc(this DishQueryRequestValue value)
        {
            var result = new GrpcContract.MenuService.DishQueryRequest();

            if (value.DishId != null) result.DishId = value.DishId;
            if (value.IsDeleted != null) result.IsDeleted = value.IsDeleted.Value;
            if (value.Name != null) result.Name = value.Name;
            if (value.DishType != null) result.DishType = (int)value.DishType;
            if (value.Description != null) result.Description = value.Description;
            

            return result;
        }

        public static Expression<Func<DishEntity, bool>> ToQuery(
            this GrpcContract.MenuService.DishQueryRequest value
            )
        {
            Expression<Func<DishEntity, bool>> result = x => true;

            if (value.HasDishId)
            {
                result = result.AndAlso(x => x.Id == value.DishId);
            }

            if (value.HasIsDeleted)
            {
                result = result.AndAlso(x => x.IsDeleted == value.IsDeleted);
            }

            if (value.HasName)
            {
                result = result.AndAlso(x => x.Name == value.Name);
            }

            if (value.HasDescription)
            {
                result = result.AndAlso(x => x.Description == value.Description);
            }

            if (value.HasDishType)
            {
                DishType dishType = (DishType)value.DishType;
                result = result.AndAlso(x => x.DishType == dishType);
            }

            return result;
        }

    }
}
