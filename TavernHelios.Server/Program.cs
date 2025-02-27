using MenuServiceServer.Extensions;
using Serilog;
using TavernHelios.Server.Extensions;
using TavernHelios.Server.Middleware;
using TavernHelios.Server.Services;

namespace TavernHelios.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Добавляем поддержку кодировок, включая windows-1252 для iTextSharp
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder(args);

            // ��������� CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049", "https://localhost:5555", "https://localhost:8888", "http://178.72.83.217:32040", "https://tavernhelios.duckdns.org") // ��������� ��������� ����������
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
            builder.Services.AddScoped<ReservationExportService>();

            Log.Logger = new LoggerConfiguration()
                .Configure(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

                app.UseSwagger();
                app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<LoggingMiddleware>();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
