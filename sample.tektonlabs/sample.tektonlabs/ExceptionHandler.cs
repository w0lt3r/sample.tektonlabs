using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace sample.tektonlabs.webapi
{
    public static class ExceptionHandler
    {
        public static async Task HandleException(this HttpContext httpContext, ILogger<object> logger)
        {
            httpContext.Response.ContentType = "application/json";
            var ex = httpContext.Features.Get<IExceptionHandlerFeature>();
            //logger.LogError(ex.Error.StackTrace);
            var response = new { Message = ex.Error.Message };
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(
                response, serializeOptions
                ));
        }
    }
}
