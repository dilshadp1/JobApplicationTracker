using FluentValidation;

namespace JobTracker.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is ValidationException validationEx)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                object response = new { validationEx.Errors };
                await context.Response.WriteAsJsonAsync(response);
            }
            else if (ex is UnauthorizedAccessException unauthorizedEx )
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                object response = new { unauthorizedEx.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
            else if (ex is KeyNotFoundException keyNotFoundEx)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                object response = new { keyNotFoundEx.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                object response;

                if (_env.IsDevelopment())
                {
                    response = new { Message = "Internal Server Error", Details = ex.InnerException?.Message ?? ex.Message };
                }
                else
                {
                    response = new { Message = "Internal Server Error" };
                }
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
