using System.Text;
using System.Text.Json;
using TavernHelios.AdministrationService.APICore.Entities;

namespace TavernHelios.AdministrationService.APICore.Converters
{
    public static class ToLogEntityConverter
    {
        public static LogEntity? ToLogEntity(byte[] arr)
        {
            var message = Encoding.UTF8.GetString(arr);
            return ToLogEntity(message);
        }

        public static LogEntity? ToLogEntity(string json)
        {
            var logEntity = new LogEntity();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DateTimeConverter() }
                };

                // Десериализация JSON в JsonElement
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json, options);

                // Заполнение свойств LogEntity
                logEntity.Timestamp = jsonObject
                    .GetProperty("Timestamp")
                    .GetDateTime();

                logEntity.Level = jsonObject
                    .GetProperty("Level")
                    .GetString();

                logEntity.MessageTemplate = jsonObject
                    .GetProperty("MessageTemplate")
                    .GetString();

                logEntity.Exception = jsonObject
                    .GetProperty("Exception")
                    .GetString();

                logEntity.Method = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("Method")
                    .GetProperty("Value")
                    .GetString();

                logEntity.Path = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("Path")
                    .GetProperty("Value")
                    .GetString();

                logEntity.RequestBody = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("RequestBody")
                    .GetProperty("Value")
                    .GetString();

                logEntity.StatusCode = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("StatusCode")
                    .GetProperty("Value")
                    .GetInt32();

                logEntity.ResponseBody = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("ResponseBody")
                    .GetProperty("Value")
                    .GetString();

                logEntity.Controller = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("Controller")
                    .GetProperty("Value")
                    .GetString();

                logEntity.User = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("User")
                    .GetProperty("Value")
                    .GetString();

                logEntity.Application = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("Application")
                    .GetProperty("Value")
                    .GetString();

                logEntity.SourceContext = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("SourceContext")
                    .GetProperty("Value")
                    .GetString();

                logEntity.RequestId = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("RequestId")
                    .GetProperty("Value")
                    .GetString();

                logEntity.RequestPath = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("RequestPath")
                    .GetProperty("Value")
                    .GetString();

                logEntity.ConnectionId = jsonObject
                    .GetProperty("Properties")
                    .GetProperty("ConnectionId")
                    .GetProperty("Value")
                    .GetString();

                logEntity.Exception = null; // Исключение отсутствует
            }
            catch (JsonException ex)
            {
                // Обработка ошибок десериализации JSON
                logEntity.Level = "Verbose";
                logEntity.Exception = $"JSON parsing error: {ex.Message}";
            }
            catch (KeyNotFoundException ex)
            {
                // Обработка отсутствия обязательных свойств
                logEntity.Level = "Verbose";
                logEntity.Exception = $"Missing property in JSON: {ex.Message}";
            }
            catch (InvalidOperationException ex)
            {
                // Обработка ошибок при извлечении значений (например, неверный тип данных)
                logEntity.Level = "Verbose";
                logEntity.Exception = $"Invalid operation: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Обработка всех остальных исключений
                logEntity.Level = "Verbose";
                logEntity.Exception = $"Unexpected error: {ex.Message}";
            }

            return logEntity;
        }
    }
}
