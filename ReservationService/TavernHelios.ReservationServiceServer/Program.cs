
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReservationServiceServer.Extensions;
using ReservationServiceServer.ReservationService;
using System.Net;
using TavernHelios.GrpcCommon.Settings;

namespace TavernHelios.ReservationServiceServer
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
                var settings = options.ApplicationServices.GetRequiredService<IOptions<GrpcReservationServiceSettings>>().Value;
                var address = IPAddress.Parse(settings.Ip);
                var port = settings.Port;
                options.Listen(address, port, (o) =>
                {
                    o.Protocols = HttpProtocols.Http2;
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
            app.MapGrpcReflectionService();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                //закомментировать для миграций
               //var task = db.Database.EnsureDeletedAsync();
               // task.Wait();
               // Console.WriteLine("Database deleted");
               // task = db.Database.EnsureCreatedAsync();
               // task.Wait();
               // Console.WriteLine("Database created");

                //расскомментировать для миграций
                //метод мигрирования взят отсюда: https://www.learnentityframeworkcore.com/migrations/add-migration
                db.Database.Migrate();
            }

            var settings = app.Services.GetRequiredService<IOptions<GrpcReservationServiceSettings>>().Value;
            app.MapGrpcService<ReservationServiceApi>().RequireHost($"*:{settings.Port}");

            app.Run();
        }
    }
}
