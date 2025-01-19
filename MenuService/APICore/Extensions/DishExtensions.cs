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
            return result;
        }

        public static DishEntity ToEntity(this DishValue menuValue)
        {
            var result = new DishEntity();
            result.Id = menuValue.Id;
            result.Description = menuValue.Description;
            return result;
        }

    }
}
