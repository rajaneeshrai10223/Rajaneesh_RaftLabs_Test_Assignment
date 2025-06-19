using System.Net;
using System.Text.Json;

namespace User.Processing.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException httpEx)
            {
                await HandleExceptionAsync(context, httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception thrown.");
                var errorResponse = new
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Error = "Internal Server Error",
                    Message = "Internal Server Error. Please try again later."
                };
                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(jsonResponse);
            }

        }

        // Centralized function to handle exceptions 
        private async Task HandleExceptionAsync(HttpContext context, HttpRequestException exception)
        {
            _logger.LogError(exception, "Unhandled exception thrown.");

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var error = "Internal Server Error";
            var message = "Internal Server Error. Please try again later.";

            if (exception.StatusCode.HasValue)
            {
                switch (exception.StatusCode.Value)
                {
                    case HttpStatusCode.BadRequest:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        error = "Bad Request";
                        message = exception.Message;
                        break;
                    case HttpStatusCode.NotFound:
                        statusCode = (int)HttpStatusCode.NotFound;
                        error = "Resource Not Found";
                        message = "User not found.";
                        break;
                    case HttpStatusCode.RequestTimeout:
                        statusCode = (int)HttpStatusCode.RequestTimeout;
                        error = "Request Timeout";
                        message = "The request has timed out. Please try again later.";
                        break;
                    default:
                        statusCode = (int)exception.StatusCode.Value;
                        error = exception.StatusCode.Value.ToString();
                        message = exception.Message;
                        break;
                }
            }

            var errorResponse = new
            {
                Status = statusCode,
                Error = error,
                Message = message
            };
            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}