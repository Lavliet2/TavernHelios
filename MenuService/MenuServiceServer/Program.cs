using System.Net;
using MenuServiceServer.Extensions;
using MenuServiceServer.MenuService;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.MenuService.Common.Settings;
//using Grpc.AspNetCore.Server.Reflection;

namespace MenuServiceServer
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
                var settings = options.ApplicationServices.GetRequiredService<IOptions<GrpcMenuServiceSettings>>().Value;
                var address = IPAddress.Parse(settings.Ip);
                var port = settings.Port;
                options.Listen(address, port, (o) =>
                {
                    o.Protocols = HttpProtocols.Http2;
                });
                Console.WriteLine($"MenuService: {settings.Ip}:{settings.Port}");
            });
           

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
                app.MapGrpcReflectionService();

            var settings = app.Services.GetRequiredService<IOptions<GrpcMenuServiceSettings>>().Value;
            app.MapGrpcService<MenuServiceApi>().RequireHost($"*:{settings.Port}");


            var mongoSettings = app.Services.GetRequiredService<IOptions<MongoConnectionSettings>>().Value;
            if(mongoSettings.ReInitDbWithTestData)
            {
                Task.WaitAll(app.FillMockDataAsync());
            }
            

            app.Run();

        }
    }
}
