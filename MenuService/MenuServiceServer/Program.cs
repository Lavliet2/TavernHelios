using System.Net;
using MenuServiceServer.Extensions;
using MenuServiceServer.MenuService;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using TavernHelios.GrpcCommon.Settings;

namespace MenuServiceServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddGrpc();

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
            });

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();
            app.UseCors("AllowFrontend");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }

            var settings = app.Services.GetRequiredService<IOptions<GrpcMenuServiceSettings>>().Value;
            app.MapGrpcService<MenuServiceApi>().RequireHost($"*:{settings.Port}");

            Task.WaitAll(app.FillMockDataAsync());

            app.Run();

        }
    }
}
