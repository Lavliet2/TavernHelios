using Grpc.Core;
using GrpcContract;
using GrpcContract.WeatherService;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TavernHelios.WeatherService.APICore.DTOValues;
using TavernHelios.WeatherService.APICore.Extensions;

namespace TavernHelios.WeatherServiceServer.WeatherService;

public class WeatherService : GrpcContract.WeatherService.WeatherService.WeatherServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDistributedCache _cache;
    private readonly IWeatherParser _weatherParser;
    private readonly ILogger<WeatherService> _logger;
    private readonly string _apiKey;

    public WeatherService(IHttpClientFactory httpClientFactory, IDistributedCache cache, ILogger<WeatherService> logger, IWeatherParser weatherParser, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
        _weatherParser = weatherParser;
        _apiKey = config["WeatherApi:Key"] ?? throw new InvalidOperationException("API key not found.");
    }

    public override async Task<WeatherReply> GetWeatherForecast(WeatherRequest request, ServerCallContext context)
    {
        var city = request.City.Trim().ToLowerInvariant();
        var currentKey = $"weather:current:{city}";
        var forecastKey = $"weather:forecast:{city}";

        var currentJson = await _cache.GetStringAsync(currentKey);
        var forecastJson = await _cache.GetStringAsync(forecastKey);

        if (!string.IsNullOrWhiteSpace(currentJson) && !string.IsNullOrWhiteSpace(forecastJson))
        {
            _logger.LogInformation("Serving cached weather for {City}", city);

            var current = JsonSerializer.Deserialize<WeatherReplyDto>(currentJson)!;
            var forecast = JsonSerializer.Deserialize<WeatherReplyDto>(forecastJson)!;

            var reply = forecast.ToGrpc();
            if (current.Today.Any())
            {
                reply.Today.Insert(0, current.Today.First());
            }

            return reply;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days=3&lang=ru");

            if (!response.IsSuccessStatusCode)
            {
                return new WeatherReply
                {
                    State = ReplyState.Error,
                    Messages = { "Не удалось получить данные с погодного сервиса" }
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            //var weather = ParseWeather(json);
            var weather = _weatherParser.ParseWeather(json);

            var now = DateTimeOffset.Now;
            var midnight = now.Date.AddDays(1);

            var currentDto = new WeatherReplyDto
            {
                Today = new List<WeatherEntry> { weather.Today[0] },
                State = ReplyState.Ok
            };

            await _cache.SetStringAsync(
                currentKey,
                JsonSerializer.Serialize(currentDto),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

            var forecastOnly = weather.ToDto();
            forecastOnly.Today = forecastOnly.Today.Skip(1).ToList();

            await _cache.SetStringAsync(
                forecastKey,
                JsonSerializer.Serialize(forecastOnly),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = midnight
                });

            return weather;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении прогноза погоды");
            return new WeatherReply
            {
                State = ReplyState.Error,
                Messages = { "Внутренняя ошибка сервиса погоды" }
            };
        }
    }
}
