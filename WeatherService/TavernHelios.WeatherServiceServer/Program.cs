using TavernHelios.WeatherServiceServer.Extensions;
using TavernHelios.WeatherServiceServer.WeatherService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureWeatherServices(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapGrpcService<WeatherService>();
app.MapGet("/", () => "Weather gRPC service is running.");

app.Run();
