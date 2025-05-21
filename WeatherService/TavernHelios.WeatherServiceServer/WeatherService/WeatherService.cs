using Grpc.Core;
using GrpcContract;
using GrpcContract.WeatherService;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TavernHelios.WeatherServiceServer.WeatherService;

public class WeatherService : GrpcContract.WeatherService.WeatherService.WeatherServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDistributedCache _cache;
    private readonly ILogger<WeatherService> _logger;
    private readonly string _apiKey;

    public WeatherService(IHttpClientFactory httpClientFactory, IDistributedCache cache, ILogger<WeatherService> logger, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
        _apiKey = config["WeatherApi:Key"] ?? throw new InvalidOperationException("API key not found.");
    }

    public override async Task<WeatherReply> GetWeatherForecast(WeatherRequest request, ServerCallContext context)
    {
        var city = request.City.Trim().ToLowerInvariant();
        var cacheKey = $"weather:{city}";
        string? cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrWhiteSpace(cached))
        {
            _logger.LogInformation("Serving cached weather for {City}", city);
            return JsonSerializer.Deserialize<WeatherReply>(cached)!;
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
            var weather = ParseWeather(json);

            // Кэшируем на 10 минут
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(weather), options);

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

    private WeatherReply ParseWeather(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var location = root.GetProperty("location");
        var forecast = root.GetProperty("forecast").GetProperty("forecastday");

        string GetDate(JsonElement day) => day.GetProperty("date").GetString() ?? "";

        string GetCondition(JsonElement el) => el.GetProperty("condition").GetProperty("text").GetString() ?? "";

        WeatherEntry FindHour(JsonElement day, string hourLabel, string timeStr)
        {
            var hour = day.GetProperty("hour").EnumerateArray()
                .FirstOrDefault(h => h.GetProperty("time").GetString()!.EndsWith(timeStr));
            return new WeatherEntry
            {
                Label = hourLabel,
                TemperatureC = hour.GetProperty("temp_c").GetDouble(),
                Condition = GetCondition(hour)
            };
        }

        var today = forecast[0];
        var tomorrow = forecast.GetArrayLength() > 1 ? forecast[1] : today;
        var afterTomorrow = forecast.GetArrayLength() > 2 ? forecast[2] : today;

        return new WeatherReply
        {
            City = location.GetProperty("name").GetString() ?? "",
            TodayDate = GetDate(today),
            TomorrowDate = GetDate(tomorrow),
            AfterTomorrowDate = GetDate(afterTomorrow),

            Today = {
            new WeatherEntry {
                Label = "Сейчас",
                TemperatureC = root.GetProperty("current").GetProperty("temp_c").GetDouble(),
                Condition = GetCondition(root.GetProperty("current")),
                Bold = true
            },
            FindHour(today, "Сегодня в 12:00", "12:00"),
            FindHour(today, "Сегодня в 13:00", "13:00")
        },
            Tomorrow = {
            FindHour(tomorrow, "Завтра в 12:00", "12:00"),
            FindHour(tomorrow, "Завтра в 13:00", "13:00")
        },
            TomorrowSummary = new WeatherSummary
            {
                AvgTempC = tomorrow.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                Condition = GetCondition(tomorrow.GetProperty("day"))
            },
            AfterTomorrow = {
            FindHour(afterTomorrow, "Послезавтра в 12:00", "12:00"),
            FindHour(afterTomorrow, "Послезавтра в 13:00", "13:00")
        },
            AfterTomorrowSummary = new WeatherSummary
            {
                AvgTempC = afterTomorrow.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                Condition = GetCondition(afterTomorrow.GetProperty("day"))
            },
            State = ReplyState.Ok
        };
    }
}
