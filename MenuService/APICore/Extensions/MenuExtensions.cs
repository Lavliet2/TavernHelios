using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.DTOValues.Menu;
using APICore.Interfaces;
using MongoRepositories.Entities;

namespace APICore.Extensions
{
    public static class MenuExtensions
    {
        public static MenuValue ToDto(this MenuEntity entity)
        {
            var result = new MenuValue();
            result.Id = entity.Id;
            result.Name = entity.Name;
            result.Dishes = entity.Dishes;
            return result;
        }

        public static  async Task<MenuValueFull> ToFullDtoAsync(this MenuEntity entity, IRepository<DishEntity> dishRepository)
        {
            var result = new MenuValueFull();
            result.Id = entity.Id;
            result.Name = entity.Name;
            result.Dishes = entity.Dishes;
            List<DishValue> fullDishesList = new List<DishValue>();

            foreach(var dishId in result.Dishes)
            {
                var dish = await dishRepository.GetByIdAsync(dishId);
                fullDishesList.Add(dish.ToDto());
            }
            result.DishesFull = fullDishesList;
            return result;
        }

        public static MenuEntity ToEntity(this MenuValue menuValue)
        {
            var result = new MenuEntity();
            result.Id = menuValue.Id;
            result.Name = menuValue.Name;
            result.Dishes = menuValue.Dishes;
            return result;
        }


    }
}
