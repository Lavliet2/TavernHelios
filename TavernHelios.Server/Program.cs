using MenuServiceServer.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                        builder.WithOrigins("https://localhost:63049", "https://localhost:5555", "http://localhost:8889", "http://178.72.83.217:32040", "https://tavernhelios.duckdns.org", "https://localhost:7190") // ��������� ��������� ����������
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
            builder.Services.AddScoped<ReservationExportService>();
            builder.Services.AddHttpClient<IAuthAPIService, AuthAPIService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["AuthServiceUrl"]);
            });
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.LoginPath = "/api/auth/login"; // Путь для перенаправления на страницу входа
                    options.Cookie.Name = "auth_cookie";
                    //options.AccessDeniedPath = "/api/auth/accessdenied"; // Путь для перенаправления при отказе в доступе
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.HttpOnly = false;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            // Отменяем редирект и возвращаем 401
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                    };
                });

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
