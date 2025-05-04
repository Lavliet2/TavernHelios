using Serilog;
using Serilog.Context;
using System.Text;

namespace TavernHelios.Server.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Добавляем контекст для логов
            using (LogContext.PushProperty("SourceContext", "Middleware"))
            {
                // Читаем тело запроса
                var requestBody = await FormatRequest(context.Request);

                // Получаем информацию о контроллере, действии и пользователе
                var username = context.User.Identity?.Name ?? "Anonymous";

                // Получаем данные маршрутизации
                var routeData = context.GetRouteData();
                var controllerName = routeData.Values["controller"]?.ToString();
                var actionName = routeData.Values["action"]?.ToString();

                // Копируем тело ответа для логирования
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        // Передаем управление следующему middleware
                        await _next(context);

                        // Логируем информацию о ответе, включая данные запроса
                        var responseBodyText = await FormatResponse(context.Response);
                        if (context.Response.StatusCode == 200)
                        {
                            Log.Information(
                                "HTTP Response: Method: {Method}, Path: {Path}, Request Body: {RequestBody}, " +
                                "Status Code: {StatusCode}, Response Body: {ResponseBody}, " +
                                "Controller: {Controller}, Action: {Action}, User: {User}",
                                context.Request.Method,
                                context.Request.Path,
                                requestBody,
                                context.Response.StatusCode,
                                responseBodyText,
                                controllerName,
                                actionName,
                                username
                            );
                        }
                        else
                        {
                            Log.Error(
                                "HTTP Response: Method: {Method}, Path: {Path}, Request Body: {RequestBody}, " +
                                "Status Code: {StatusCode}, Response Body: {ResponseBody}, " +
                                "Controller: {Controller}, Action: {Action}, User: {User}",
                                context.Request.Method,
                                context.Request.Path,
                                requestBody,
                                context.Response.StatusCode,
                                responseBodyText,
                                controllerName,
                                actionName,
                                username
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        // Логируем исключения с уровнем Verbose
                        Log.Verbose(
                            ex,
                            "HTTP Exception: Method: {Method}, Path: {Path}, Request Body: {RequestBody}, " +
                                "Status Code: {StatusCode}, Response Body: {ResponseBody}, " +
                                "Controller: {Controller}, Action: {Action}, User: {User}",
                                context.Request.Method,
                                context.Request.Path,
                                requestBody,
                                context.Response.StatusCode,
                                "",
                                controllerName,
                                actionName,
                                username
                        );

                        // Пробрасываем исключение дальше
                        throw;
                    }
                    finally
                    {
                        // Копируем тело ответа обратно в оригинальный поток
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}
