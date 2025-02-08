using System.Net.Http;
using MenuServiceServer.Extensions;

namespace TavernHelios.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ��������� CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049", "https://localhost:5555", "https://localhost:8888", "http://178.72.83.217:32040") // ��������� ��������� ����������
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowAnyOrigin();
                               //.AllowCredentials();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
            }
                app.UseSwagger();
                app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
