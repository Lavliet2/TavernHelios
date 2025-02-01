using APICore.Interfaces;
using APICore.Settings;
using Microsoft.OpenApi.Models;
using MongoRepositories.Entities;
using MongoRepositories.Interfaces;
using MongoRepositories.MockData;
using MongoRepositories.Repositories;

namespace MenuServiceServer.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Заполнение БД тестовыми данными, если она пустая
        /// </summary>
        /// <param name="app"></param>
        public static async Task FillMockDataAsync(this WebApplication app)
        {
            var mockWriter = app.Services.GetRequiredService<IDbMockDataWriter<MenuEntity>>();
            if (await mockWriter.IsNeedFillMockDataAsync())
                await mockWriter.FillDbWithMockDataAsync();
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddSettings(configuration);
            services.AddDbConfiguration();
            services.AddRepositories();
            services.AddCaches();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Menu Service Api", Version = "v1" });
            });
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
            services.AddSingleton<IRepository<DishEntity>, DishRepository>();
            services.AddSingleton<IRepository<MenuEntity>, MenuRepository>();

            ///Для заполнения тестовыми данными
            services.AddSingleton<IDbMockDataWriter<DishEntity>, DishMockData>();
            services.AddSingleton<IDbMockDataWriter<MenuEntity>, MenuMockData>();
        }

        private static void AddDbConfiguration(this IServiceCollection services)
        {
            //services.AddSingleton<IDBInitializer, DBInitializer.DBInitializer>();
            //services.AddSingleton<IDBCreator, DBCreator>(); 
        }

        private static void AddCaches(this IServiceCollection services)
        {
            //services.AddSingleton<ICache<MenuEntity>, MenuCache>();
        }

        
        private static void AddSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoConnectionSettings>(configuration.GetSection(nameof(MongoConnectionSettings)));
        }
    }
}
