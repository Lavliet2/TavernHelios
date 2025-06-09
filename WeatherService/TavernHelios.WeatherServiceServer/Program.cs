using Autofac;
using Autofac.Extensions.DependencyInjection;
using TavernHelios.WeatherServiceServer.Extensions;
using TavernHelios.WeatherServiceServer.WeatherService;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<WeatherParser>()
        .As<IWeatherParser>()
        .SingleInstance();
});
builder.Services.ConfigureWeatherServices(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapGrpcService<WeatherService>();
app.MapGet("/", () => "Weather gRPC service is running.");

app.Run();
