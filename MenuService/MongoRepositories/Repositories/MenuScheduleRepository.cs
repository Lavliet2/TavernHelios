using Microsoft.Extensions.Options;
using TavernHelios.MenuService.APICore.Entities.Menu;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Interfaces;
using TavernHelios.MenuService.Common.Settings;

namespace TavernHelios.MenuService.MongoRepositories.Repositories
{
    public class MenuScheduleRepository : BaseMongoRepository<MenuScheduleEntity>
    {
        private readonly IRepository<MenuEntity> _menuRepository;
        public MenuScheduleRepository(
            IOptions<MenuMongoConnectionSettings> mongoSettings,
            IRepository<MenuEntity> menuRepository
            ) : base(mongoSettings)
        {
            _menuRepository = menuRepository;
        }
    }
}
