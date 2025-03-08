using Microsoft.AspNetCore.Diagnostics;
using System.Security.Authentication;

namespace TavernHelios.Server.Exceptions
{
    public class ExceptionMiddleware
    {
        public static void HandleError(IApplicationBuilder options)
        {
            options.Run(
                async context =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";
                    var e = context.Features.Get<IExceptionHandlerFeature>();
                    if (e != null)
                    {
                        var ex = e.Error;
                        string message = "Произошла ошибка!";

                        var model = new ExceptionDTO
                        {
                            Message = message,
                        };

                        while (ex.InnerException != null) ex = ex.InnerException;

                        if (ex is CustomException custom)
                        {
                            if (!string.IsNullOrEmpty(custom.Description))
                                model.Message = custom.Description;
                        }

                        await context.Response.WriteAsJsonAsync(model);
                    }
                });
        }

    }
}
