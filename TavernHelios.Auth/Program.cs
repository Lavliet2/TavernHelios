using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TavernHelios.Auth.Data;
using TavernHelios.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(connection));


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.LoginPath = "/api/auth/login"; // Путь для перенаправления на страницу входа
        //options.AccessDeniedPath = "/api/auth/accessdenied"; // Путь для перенаправления при отказе в доступе
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
