using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Entities;
using APICore.Enums;


namespace MongoRepositories.Entities
{
    /// <summary>
    /// Сущность Блюда, хранимая в БД
    /// </summary>
    public class DishEntity : IEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public DishType DishType { get; set; }
    }
}
