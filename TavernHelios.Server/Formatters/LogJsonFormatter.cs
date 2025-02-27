using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace TavernHelios.Server.Formatters
{
    public class LogJsonFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            // Создаем объект для сериализации
            var logObject = new
            {
                Timestamp = logEvent.Timestamp.ToString("o"), // Время в формате ISO 8601
                Level = logEvent.Level.ToString(), // Уровень лога (Information, Error и т.д.)
                MessageTemplate = logEvent.MessageTemplate.Text, // Шаблон сообщения
                Properties =
                    logEvent
                    .Properties
                    .ToDictionary(p => p.Key, p => p.Value), // Свойства лога
                Exception = logEvent.Exception?.ToString() // Исключение (если есть)
            };

            var json = JsonConvert.SerializeObject(logObject, Formatting.None);

            output.WriteLine(json);
        }
    }
}
