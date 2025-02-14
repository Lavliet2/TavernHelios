using MenuServiceServer.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using TavernHelios.Server.Data;
using TavernHelios.Server.Services.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Настройка CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder.WithOrigins("http://localhost:63049", "http://localhost:5555", "http://localhost:8888", "http://178.72.83.217:32050", "https://tavernhelios.duckdns.org") 
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
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddYandex("Yandex", options =>
            {
                options.ClientId = "…";
                options.ClientSecret = "…";
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

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<EfDbContext>();
                context.Database.Migrate();  // 🔹 Накатываем миграции автоматически
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка применения миграций: {ex.Message}");
            }
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

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
