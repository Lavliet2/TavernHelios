using Microsoft.EntityFrameworkCore;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.ApiCore.Settings;
using TavernHelios.ReservationService.APICore.Entities;
using TavernHelios.ReservationService.PostgreRepository;
using TavernHelios.ReservationService.PostgreRepository.Repositories.EF;

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
            //Connection string goes into context constructor via DI

            services.AddDbContext<DatabaseContext>();
            services.AddScoped<DbContext>(p => p.GetRequiredService<DatabaseContext>());
            
            services.AddScoped<IRepository<ReservationEntity>, ReservationRepository>();
            //services.AddSingleton<IRepository<ReservationEntity>, ReservationRepository_Mock>();
        }

        private static void AddDbConfiguration(this IServiceCollection services)
        {
           
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
            services.Configure<GrpcReservationServiceSettings>(configuration.GetSection(nameof(GrpcReservationServiceSettings)));
        }
    }
}
