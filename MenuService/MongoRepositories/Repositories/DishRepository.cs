using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernHelios.MenuService.ApiCore.Settings;
using Microsoft.Extensions.Options;
using MongoRepositories.Entities;

namespace MongoRepositories.Repositories
{
    public class DishRepository : BaseMongoRepository<DishEntity>
    {
        public DishRepository(IOptions<MongoConnectionSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}
