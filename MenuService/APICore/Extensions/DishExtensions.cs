using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.DTOValues.Menu;
using MongoRepositories.Entities;

namespace APICore.Extensions
{
    public static class DishExtensions
    {
        public static DishValue ToDto(this DishEntity entity)
        {
            var result = new DishValue();
            result.Id = entity.Id;
            result.Description = entity.Description;
            result.Name = entity.Name;
            result.ImageBase64 = entity.ImageBase64;
            result.DishType = entity.DishType;
            return result;
        }

        public static DishEntity ToEntity(this DishValue dishValue)
        {
            var result = new DishEntity();
            result.Id = dishValue.Id;
            result.Name= dishValue.Name;
            result.DishType = dishValue.DishType;
            result.Description = dishValue.Description;
            result.ImageBase64 = dishValue.ImageBase64;
            return result;
        }

    }
}
