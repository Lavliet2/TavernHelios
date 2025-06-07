using System.Text.Json;
using GrpcContract;
using GrpcContract.WeatherService;

namespace TavernHelios.WeatherServiceServer.WeatherService
{
    public class WeatherParser : IWeatherParser
    {
        public WeatherReply ParseWeather(string json)
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

                if (hour.ValueKind == JsonValueKind.Undefined)
                    return new WeatherEntry { Label = hourLabel, TemperatureC = 0, Condition = "N/A" };

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
                TodaySummary = new WeatherSummary
                {
                    AvgTempC = today.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                    MaxTempC = today.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                    MinTempC = today.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                    Humidity = today.GetProperty("day").GetProperty("avghumidity").GetDouble(),
                    WindKph = today.GetProperty("day").GetProperty("maxwind_kph").GetDouble(),
                    Condition = GetCondition(today.GetProperty("day"))
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
                    Condition = GetCondition(tomorrow.GetProperty("day"))
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
                    Condition = GetCondition(afterTomorrow.GetProperty("day"))
                },
                State = ReplyState.Ok
            };
        }
    }
}
