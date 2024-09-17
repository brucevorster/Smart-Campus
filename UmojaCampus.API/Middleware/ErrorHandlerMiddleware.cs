using UmojaCampus.Shared.Wrapper;
using UmojaCampus.Shared.Exceptions;
using System.Net;
using System.Text.Json;
namespace UmojaCampus.API.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = Result<string>.Fail(error.Message);

                response.StatusCode = error switch
                {
                    ApiException => (int) HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int) HttpStatusCode.NotFound,
                    _ => (int) HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
