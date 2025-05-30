﻿using Grpc.Core;
using Microsoft.OpenApi.Models;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.RabbitMq.Settings;
using TavernHelios.Server;
using TavernHelios.Server.Services.ReservedLayoutService;
using static GrpcContract.LayoutService.LayoutService;
using static GrpcContract.MenuService.MenuService;
using static GrpcContract.ReservationService.ReservationService;
using static GrpcContract.WeatherService.WeatherService;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tavern Helios Main Api", Version = "v1" });
            });
        }

        public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<MenuServiceClient, GrpcMenuServiceSettings>(configuration);
            services.AddGrpcClient<ReservationServiceClient, GrpcReservationServiceSettings>(configuration);
            services.AddGrpcClient<LayoutServiceClient, GrpcLayoutServiceSettings>(configuration);
            services.AddGrpcClient<WeatherServiceClient, GrpcWeatherServiceSettings>(configuration);
        }

        private static void AddGrpcClient<TService, TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TSettings:  GrpcSettingsBase
            where TService : class
        {
            //Добавляем GRPC клиента
            var grpcSettings = configuration.GetSection(typeof(TSettings).Name).Get<TSettings>();

            var channelCredentials = ChannelCredentials.Insecure;
            var address = new Uri($"http://{grpcSettings.Ip}:{grpcSettings.Port}");
            services.AddGrpcClient<TService>(o =>
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
            services.AddSingleton<ReservedLayoutService, ReservedLayoutService>();
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
            services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        }
    }
}
