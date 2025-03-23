
using LayoutServiceServer.Extensions;
using LayoutServiceServer.LayoutService;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using System.Net;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.LayoutService.Common.Settings;

namespace TavernHelios.LayoutService.LayoutServiceServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();
            builder.Services.AddGrpcReflection();

            builder.Services.ConfigureServices(builder.Configuration);

            builder.WebHost.ConfigureKestrel(options =>
            {
                var settings = options.ApplicationServices.GetRequiredService<IOptions<GrpcLayoutServiceSettings>>().Value;
                var address = IPAddress.Parse(settings.Ip);
                var port = settings.Port;
                options.Listen(address, port, (o) =>
                {
                    o.Protocols = HttpProtocols.Http2;
                });
                Console.WriteLine($"LayoutService: {settings.Ip}:{settings.Port}");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
            app.MapGrpcReflectionService();

            var settings = app.Services.GetRequiredService<IOptions<GrpcLayoutServiceSettings>>().Value;
            app.MapGrpcService<LayoutServiceApi>().RequireHost($"*:{settings.Port}");


            var mongoSettings = app.Services.GetRequiredService<IOptions<LayoutMongoConnectionSettings>>().Value;

            app.CheckConnectionAsync();

            app.Run();

        }
    }
}
