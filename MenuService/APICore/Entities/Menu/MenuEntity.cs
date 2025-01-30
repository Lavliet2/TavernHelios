using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Entities;

namespace MongoRepositories.Entities
{
    /// <summary>
    /// Сущность Меню, хранимая в БД
    /// </summary>
    public class MenuEntity : IEntity
    {
        public string Id { get; set; } = String.Empty;

        public List<string> Dishes { get; set; } = new List<string>();
    }
}
