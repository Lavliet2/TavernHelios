using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Enums;
using APICore.Interfaces;
using MongoRepositories.Entities;
using MongoRepositories.Interfaces;

namespace MongoRepositories.MockData
{
    public class MenuMockData : MockDataWriterBase<MenuEntity>
    {
        private readonly IRepository<DishEntity> _dishRepository;
        public MenuMockData(
            IRepository<MenuEntity> menuRepository,
            IRepository<DishEntity> dishRepository
            ) : base(menuRepository)
        {
            _dishRepository = dishRepository;
        }

        public override async Task<IEnumerable<MenuEntity>> FillDbWithMockDataAsync()
        {
            Console.WriteLine("Заполнение БД Меню тестовыми данными BEGIN");
            var result = new List<MenuEntity>();

            var dishMockData = new DishMockData(_dishRepository);
            if(await dishMockData.IsNeedFillMockDataAsync())
            {
                await dishMockData.FillDbWithMockDataAsync();
            }

            var allDishes = await _dishRepository.GetAllAsync();

            var menu1 = new MenuEntity()
            {
                Date = DateTime.Now.Date,
                Description = "Тест меню 1"
            };
            menu1.Dishes = new List<string>();
            menu1.Dishes.Add(allDishes.FirstOrDefault(x => x.DishType == DishType.FirstDish)?.Id ?? string.Empty);
            menu1.Dishes.Add(allDishes.FirstOrDefault(x => x.DishType == DishType.SecondDish)?.Id ?? string.Empty);
            menu1.Dishes.Add(allDishes.FirstOrDefault(x => x.DishType == DishType.Kompot)?.Id ?? string.Empty);
            result.Add( await _repository.CreateAsync(menu1));

            var menu2 = new MenuEntity()
            {
                Date = DateTime.Now.Date,
                Description = "Тест меню 2"
            };
            menu2.Dishes = new List<string>();
            menu2.Dishes.Add(allDishes.LastOrDefault(x => x.DishType == DishType.FirstDish)?.Id ?? string.Empty);
            menu2.Dishes.Add(allDishes.LastOrDefault(x => x.DishType == DishType.SecondDish)?.Id ?? string  .Empty);
            menu2.Dishes.Add(allDishes.LastOrDefault(x => x.DishType == DishType.Kompot)?.Id ?? string.Empty);
            result.Add(await _repository.CreateAsync(menu2));

            var menu3 = new MenuEntity()
            {
                Date = DateTime.Now.Date,
                Description = "Тест меню 3"
            };
            menu3.Dishes = new List<string>();
            menu3.Dishes.Add(allDishes.FirstOrDefault(x => x.DishType == DishType.FirstDish)?.Id ?? string.Empty);
            menu3.Dishes.Add(allDishes.LastOrDefault(x => x.DishType == DishType.SecondDish)?.Id ?? string.Empty);
            menu3.Dishes.Add(allDishes.FirstOrDefault(x => x.DishType == DishType.Kompot)?.Id ?? string.Empty);
            result.Add( await _repository.CreateAsync(menu3));

            Console.WriteLine("Заполнение БД Меню тестовыми данными END");

            return result;
        }

        
    }
}
