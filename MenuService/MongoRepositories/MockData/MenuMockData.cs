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

            var deletedCount = await _repository.DeleteAll();
            var result = new List<MenuEntity>();

            var dishMockData = new DishMockData(_dishRepository);
            if(await dishMockData.IsNeedFillMockDataAsync())
            {
                await dishMockData.FillDbWithMockDataAsync();
            }

            var allDishes = await _dishRepository.GetAllAsync();

            //Пока что для теста создадим 2 одинаковых меню с выбором из 2 блюд каждого типа
            var menu1 = new MenuEntity() { Name = "Меню №1"};
            menu1.Dishes = new List<string>(allDishes.Select(x => x.Id));
            result.Add( await _repository.CreateAsync(menu1));

            var menu2 = new MenuEntity() { Name = "Меню №2" };
            menu2.Dishes = new List<string>(allDishes.Select(x => x.Id));
            result.Add(await _repository.CreateAsync(menu2));

            Console.WriteLine("Заполнение БД Меню тестовыми данными END");

            return result;
        }

        
    }
}
