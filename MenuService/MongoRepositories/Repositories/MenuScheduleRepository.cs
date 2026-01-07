using Microsoft.Extensions.Configuration;
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
            IConfiguration configuration,
            IRepository<MenuEntity> menuRepository
            ) : base(mongoSettings, configuration)
        {
            _menuRepository = menuRepository;
        }
    }
}
