using Newtonsoft.Json;
using System.Net;
using UserService.Exceptions;

namespace UserService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        internal class ErrorDetails
        {
            public string? Message { get; set; }

            public override string ToString() =>
                JsonConvert.SerializeObject(this);
        }

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, (int)StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorDetails = new ErrorDetails
            {
                Message = "An error occurred. Message: " + message
            };
            return context.Response.WriteAsync(errorDetails?.ToString() ?? "No error details");
        }
    }
}
