using Microsoft.Extensions.Options;
using TavernHelios.LayoutService.APICore.Entities.Layout;
using TavernHelios.LayoutService.Common.Settings;

namespace TavernHelios.LayoutService.MongoRepositories.Repositories
{
    public class LayoutRepository : BaseMongoRepository<LayoutEntity>
    {
        public LayoutRepository(
            IOptions<LayoutMongoConnectionSettings> mongoSettings
            ) : base(mongoSettings)
        {
            
        }
    }
}
