using TavernHelios.LayoutService.Common.Interfaces;
using TavernHelios.LayoutService.Common.Settings;
using TavernHelios.LayoutService.Common.Entities;
using TavernHelios.LayoutService.MongoRepositories.Interfaces;
using TavernHelios.LayoutService.MongoRepositories.Repositories;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.LayoutService.APICore.Entities.Layout;

namespace LayoutServiceServer.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Заполнение БД тестовыми данными, если она пустая
        /// </summary>
        /// <param name="app"></param>
        public static async Task CheckConnectionAsync(this WebApplication app)
        {
            var layoutRepository = app.Services.GetRequiredService<IRepository<LayoutEntity>>();
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddSettings(configuration);
            services.AddDbConfiguration();
            services.AddRepositories();
            services.AddCaches();
        }


        /// <summary>
        /// всё что не входит в отдельные категории или просто нет смысла выделять отдельный метод для этого
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<ISupportServices, ClientService>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<LayoutEntity>, LayoutRepository>();
            
        }

        private static void AddDbConfiguration(this IServiceCollection services)
        {
            //services.AddSingleton<IDBInitializer, DBInitializer.DBInitializer>();
            //services.AddSingleton<IDBCreator, DBCreator>(); 
        }

        private static void AddCaches(this IServiceCollection services)
        {
            //services.AddSingleton<ICache<LayoutEntity>, LayoutCache>();
        }

        
        private static void AddSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<LayoutMongoConnectionSettings>(configuration.GetSection(nameof(LayoutMongoConnectionSettings)));
            services.Configure<GrpcLayoutServiceSettings>(configuration.GetSection(nameof(GrpcLayoutServiceSettings)));
        }
    }
}
