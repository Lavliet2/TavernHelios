using Microsoft.Extensions.Configuration;
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
            IOptions<MenuMongoConnectionSettings> mongoSettings,
            IConfiguration configuration,
            IRepository<DishEntity> dishRepository
            ) : base(mongoSettings, configuration)
        {
            _dishRepository = dishRepository;
        }
    }
}
