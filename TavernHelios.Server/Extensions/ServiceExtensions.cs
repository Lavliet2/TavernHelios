using Grpc.Core;
using Microsoft.OpenApi.Models;
using System.Diagnostics.Contracts;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.Server;
using static GrpcContract.MenuService.MenuService;
using static GrpcContract.ReservationService.ReservationService;

namespace MenuServiceServer.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddSettings(configuration);
            //services.AddDbConfiguration();
            services.AddGrpcClients(configuration);
            services.AddCaches();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Menu Service Api", Version = "v1" });
            });
        }

        public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMenuServiceGrpcClient(configuration);
            services.AddReservationServiceGrpcClient(configuration);
        }
        
        private static void AddMenuServiceGrpcClient(this  IServiceCollection services, IConfiguration configuration)
        {
            //Добавляем GRPC клиента
            var grpcSettings = configuration.GetSection(nameof(GrpcMenuServiceSettings)).Get<GrpcMenuServiceSettings>();

            var channelCredentials = ChannelCredentials.Insecure;
            var address = new Uri($"http://{grpcSettings.Ip}:{grpcSettings.Port}");
            services.AddGrpcClient<MenuServiceClient>(o =>
            {
                o.Address = address;
                o.ChannelOptionsActions.Add((channelOption) =>
                {
                    channelOption.Credentials = channelCredentials;
                    channelOption.HttpHandler = new SocketsHttpHandler
                    {
                        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                        KeepAlivePingDelay = TimeSpan.FromSeconds(20),
                        KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
                        EnableMultipleHttp2Connections = true
                    };
                    channelOption.DisposeHttpClient = true;
                });
            });
        }

        private static void AddReservationServiceGrpcClient(this IServiceCollection services, IConfiguration configuration)
        {
            //Добавляем GRPC клиента
            var grpcSettings = configuration.GetSection(nameof(GrpcReservationServiceSettings)).Get<GrpcReservationServiceSettings>();

            var channelCredentials = ChannelCredentials.Insecure;
            var address = new Uri($"http://{grpcSettings.Ip}:{grpcSettings.Port}");
            services.AddGrpcClient<ReservationServiceClient>(o =>
            {
                o.Address = address;
                o.ChannelOptionsActions.Add((channelOption) =>
                {
                    channelOption.Credentials = channelCredentials;
                    channelOption.HttpHandler = new SocketsHttpHandler
                    {
                        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                        KeepAlivePingDelay = TimeSpan.FromSeconds(20),
                        KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
                        EnableMultipleHttp2Connections = true
                    };
                    channelOption.DisposeHttpClient = true;
                });
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

       

        private static void AddCaches(this IServiceCollection services)
        {
            //services.AddSingleton<ICache<MenuEntity>, MenuCache>();
        }

        
        private static void AddSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<GrpcMenuServiceSettings>(configuration.GetSection(nameof(GrpcMenuServiceSettings)));
            services.Configure<GrpcReservationServiceSettings>(configuration.GetSection(nameof(GrpcReservationServiceSettings)));
        }
    }
}
