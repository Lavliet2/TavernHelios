using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Settings;

namespace TavernHelios.MenuService.MongoRepositories.Repositories
{
    public class DishRepository : BaseMongoRepository<DishEntity>
    {
        public DishRepository(IOptions<MenuMongoConnectionSettings> mongoSettings, IConfiguration configuration) 
            : base(mongoSettings, configuration)
        {
        }
    }
}
