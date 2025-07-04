using MenuServiceServer.Extensions;
using Serilog;
using TavernHelios.Server.Extensions;
using TavernHelios.Server.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using TavernHelios.Server.Exceptions;
using TavernHelios.Server.Services;
using QuestPDF.Infrastructure;
using TavernHelios.Server.Hubs;

namespace TavernHelios.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Добавляем поддержку кодировок, включая windows-1252 для iTextSharp
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            QuestPDF.Settings.License = LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);
            // ��������� CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049", "https://localhost:5555", "http://localhost:8888", "https://localhost:8888", "http://localhost:8889", "http://178.72.83.217:32040", "https://tavernhelios.duckdns.org", "https://localhost:7190") // ��������� ��������� ����������
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               //.AllowAnyOrigin()
                               .AllowCredentials();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.AddSignalR();
            builder.Services.AddScoped<ReservationExportService>();
            builder.Services.AddHttpClient<IAuthAPIService, AuthAPIService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["AuthServiceUrl"]);
            });
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.HttpOnly = false;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();

            Log.Logger = new LoggerConfiguration()
                .Configure(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<LoggingMiddleware>();
            app.MapHub<ReservationHub>("/hubs/reservations");
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseExceptionHandler(options =>
            {
                ExceptionMiddleware.HandleError(options);
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
