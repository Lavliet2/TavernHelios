using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using TavernHelios.Auth.Data;
using TavernHelios.Auth.Services;

namespace TavernHelios.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var yandexOptions = new OAuthOptions
            {
                ClientId = "5c9531b9a1cb48628ffb70ea2582afc8",
                ClientSecret = "e57bbc5ff7474d0682a8d9b7af6ce75a"
            };
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "Yandex";
                })
                .AddCookie()
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            var connection = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(connection));


            builder.Services.AddScoped<IAuthService, YandexAuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


        