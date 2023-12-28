using DDDUtility;
using System.Text.Json;
using Utility.Constants;

namespace DDDApi.Middlewares
{
    public class GlobalErrorCatcher
    {
        private readonly RequestDelegate _next;

        public GlobalErrorCatcher(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                context.Response.ContentType = "application/json;charset=utf-8";

                var result = new ResponseModel<string>
                {
                    Data = null!,
                    Code = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(result, options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }
    }
}
