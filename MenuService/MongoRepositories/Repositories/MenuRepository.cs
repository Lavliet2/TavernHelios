using Microsoft.Extensions.Options;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Interfaces;
using TavernHelios.MenuService.Common.Settings;

namespace TavernHelios.MenuService.MongoRepositories.Repositories
{
    public class MenuRepository : BaseMongoRepository<MenuEntity>
    {
        private readonly IRepository<DishEntity> _dishRepository;

        //TODO: валидация существования блюда в базе
        public MenuRepository(
            IOptions<MongoConnectionSettings> mongoSettings,
            IRepository<DishEntity> dishRepository
            ) : base(mongoSettings)
        {
            _dishRepository = dishRepository;
        }
    }
}
