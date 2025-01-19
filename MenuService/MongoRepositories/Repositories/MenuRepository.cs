using APICore.Interfaces;
using APICore.Settings;
using Microsoft.Extensions.Options;
using MongoRepositories.Entities;

namespace MongoRepositories.Repositories
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
