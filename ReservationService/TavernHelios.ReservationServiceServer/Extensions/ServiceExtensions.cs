using TavernHelios.GrpcCommon.Settings;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.ApiCore.Settings;
using TavernHelios.ReservationService.APICore.Entities;
using TavernHelios.ReservationService.PostgreRepository;

namespace ReservationServiceServer.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddSettings(configuration);
            services.AddDbConfiguration();
            services.AddRepositories();
            services.AddCaches();

            //services.AddSwaggerGen(c =>
            //{
            //    c.EnableAnnotations();
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Menu Service Api", Version = "v1" });
            //});
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
            services.AddSingleton<IRepository<ReservationEntity>, ReservationRepository_Mock>();
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
            services.Configure<ReservationPostgreConnectionSettings>(configuration.GetSection(nameof(ReservationPostgreConnectionSettings)));
            services.Configure<GrpcMenuServiceSettings>(configuration.GetSection(nameof(GrpcMenuServiceSettings)));
        }
    }
}
