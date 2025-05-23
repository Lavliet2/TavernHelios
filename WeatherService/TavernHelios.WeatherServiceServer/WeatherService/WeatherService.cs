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
        var currentKey = $"weather:current:{city}";
        var forecastKey = $"weather:forecast:{city}";

        // Пытаемся собрать оба куска из кэша
        var currentJson = await _cache.GetStringAsync(currentKey);
        var forecastJson = await _cache.GetStringAsync(forecastKey);

        if (!string.IsNullOrWhiteSpace(currentJson) && !string.IsNullOrWhiteSpace(forecastJson))
        {
            _logger.LogInformation("Serving cached weather for {City}", city);

            var current = JsonSerializer.Deserialize<WeatherReplyDto>(currentJson)!;
            var forecast = JsonSerializer.Deserialize<WeatherReplyDto>(forecastJson)!;

            // Объединяем current + forecast в один ответ
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
            var weather = ParseWeather(json);

            var now = DateTimeOffset.Now;
            var midnight = now.Date.AddDays(1);

            // Текущая погода — 30 мин
            var currentDto = new WeatherReplyDto
            {
                Today = new List<WeatherEntry> { weather.Today[0] }, // только "Сейчас"
                State = ReplyState.Ok
            };

            await _cache.SetStringAsync(
                currentKey,
                JsonSerializer.Serialize(currentDto),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

            // Прогноз (без current) — до конца суток
            var forecastOnly = weather.ToDto();
            forecastOnly.Today = forecastOnly.Today.Skip(1).ToList(); // убираем "Сейчас"

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
                Condition = GetCondition(hour),
                IconUrl = hour.GetProperty("condition").GetProperty("icon").GetString() ?? ""
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
                    IconUrl = root.GetProperty("current").GetProperty("condition").GetProperty("icon").GetString() ?? "",
                    Bold = true
                },
                FindHour(today, "Сегодня в 12:00", "12:00"),
                FindHour(today, "Сегодня в 13:00", "13:00")
            },
            TodaySummary = new WeatherSummary
            {
                AvgTempC = today.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                MaxTempC = today.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                MinTempC = today.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                Humidity = today.GetProperty("day").GetProperty("avghumidity").GetDouble(),
                WindKph = today.GetProperty("day").GetProperty("maxwind_kph").GetDouble(),
                Condition = GetCondition(today.GetProperty("day")),
                IconUrl = today.GetProperty("day").GetProperty("condition").GetProperty("icon").GetString() ?? ""
            },
            Tomorrow = {
                FindHour(tomorrow, "Завтра в 12:00", "12:00"),
                FindHour(tomorrow, "Завтра в 13:00", "13:00")
            },
            TomorrowSummary = new WeatherSummary
            {
                AvgTempC = tomorrow.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                MaxTempC = tomorrow.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                MinTempC = tomorrow.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                Humidity = tomorrow.GetProperty("day").GetProperty("avghumidity").GetDouble(),
                WindKph = tomorrow.GetProperty("day").GetProperty("maxwind_kph").GetDouble(),
                Condition = GetCondition(tomorrow.GetProperty("day")),
                IconUrl = today.GetProperty("day").GetProperty("condition").GetProperty("icon").GetString() ?? ""
            },
            AfterTomorrow = {
                FindHour(afterTomorrow, "Послезавтра в 12:00", "12:00"),
                FindHour(afterTomorrow, "Послезавтра в 13:00", "13:00")
            },
            AfterTomorrowSummary = new WeatherSummary
            {
                AvgTempC = afterTomorrow.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                MaxTempC = afterTomorrow.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                MinTempC = afterTomorrow.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                Humidity = afterTomorrow.GetProperty("day").GetProperty("avghumidity").GetDouble(),
                WindKph = afterTomorrow.GetProperty("day").GetProperty("maxwind_kph").GetDouble(),
                Condition = GetCondition(afterTomorrow.GetProperty("day")),
                IconUrl = today.GetProperty("day").GetProperty("condition").GetProperty("icon").GetString() ?? ""
            },
            State = ReplyState.Ok
        };


    }
}
