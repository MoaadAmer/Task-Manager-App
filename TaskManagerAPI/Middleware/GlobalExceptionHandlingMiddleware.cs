using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception occurred");
                await HandleException(httpContext, ex);
            }
        }

        private static async Task HandleException(HttpContext httpContext, Exception ex)
        {

            int statusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails()
            {
                Title = "An unexpected error occurred",
                Status = statusCode,
                Detail = ex.Message,
                Instance = httpContext.Request.Path.Value
            };

            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
