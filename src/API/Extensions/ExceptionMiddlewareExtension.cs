using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using WordBattle.API.Controllers;

namespace WordBattle.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        var t = context.Features.Get<IItemsFeature>();

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<object>(false, exceptionHandlerPathFeature?.Error.Message ?? string.Empty, null, null)));
                    }
                });
            });
        }
    }

}
