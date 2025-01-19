using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Enums;
using APICore.Interfaces;
using MongoRepositories.Entities;

namespace MongoRepositories.MockData
{
    public class DishMockData : MockDataWriterBase<DishEntity>
    {
        public DishMockData(IRepository<DishEntity> repository) : base(repository)
        {
        }

        public override async Task<IEnumerable<DishEntity>> FillDbWithMockDataAsync()
        {
            Console.WriteLine("Заполнение БД Блюд тестовыми данными BEGIN");
            var result = new List<DishEntity>();
            foreach(var item in _mockData)
            {
                var createdItem = await _repository.CreateAsync(
                    new DishEntity() { Description = item.descr, DishType = item.dishType }
                    );
                result.Add( createdItem );
            }
            Console.WriteLine("Заполнение БД Блюд тестовыми данными END");
            return result;
        }

        private readonly List<(DishType dishType, string descr)> _mockData = new()
        {
            new(){dishType = DishType.FirstDish, descr="Борщ (Borsch)" },
            new(){dishType = DishType.FirstDish, descr="Щи (Shee)" },
            new(){dishType = DishType.SecondDish, descr="Котлетки с пюрешкой (Meat and potatoes)" },
            new(){dishType = DishType.SecondDish, descr="Бублик c маком (Dope Boob-lick)" },
            new(){dishType = DishType.Kompot, descr="Компот вишневый (Cherry cumpot)" },
            new(){dishType = DishType.Kompot, descr="Компот из кураги (Shitty cumpot)" },
        };
    }
}
