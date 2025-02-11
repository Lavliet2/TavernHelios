using System.Net.Http;
using System.Security.Claims;
using MenuServiceServer.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using TavernHelios.Server.Data;
using TavernHelios.Server.DTO;
using TavernHelios.Server.Services.Auth;

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
                               //.AllowAnyOrigin();
                               .AllowCredentials();
                    });
            });

            var yandexConfig = builder.Configuration.GetSection("YandexAuth");
            var yandexOptions = new OAuthOptions
            {
                ClientId = yandexConfig.GetValue<string>("ClientId"),
                ClientSecret = yandexConfig.GetValue<string>("ClientSecret")
            };

            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Yandex";
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None; // Разрешить использование кук на разных доменах (из-за того что бек и фронт на разных портах)
            })
            .AddYandex("Yandex", options =>
            {
                options.ClientId = yandexOptions.ClientId;
                options.ClientSecret = yandexOptions.ClientSecret;

                options.AuthorizationEndpoint = "https://oauth.yandex.com/authorize";
                options.TokenEndpoint = "https://oauth.yandex.com/token";
                options.UserInformationEndpoint = "https://login.yandex.ru/info";

                options.CallbackPath = "/yandexAuth/login";
                options.SaveTokens = true;
            });
            var connection = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(connection));
            builder.Services.AddScoped<IAuthService, YandexAuthService>();

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
