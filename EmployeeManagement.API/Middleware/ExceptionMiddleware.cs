using System.Text.Json;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;

namespace EmployeeManagement.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            var statusCode = MapStatusCode(ex);
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var errorLog = new ErrorLog
            {
                StatusCode = statusCode,
                Message = ex.Message.Length > 200 ? ex.Message.Substring(0, 200) : ex.Message,
                Path = context.Request.Path,
                Method = context.Request.Method,
                User = context.User?.Identity?.Name ?? "Anonymous",
                Level = statusCode >= 500 ? "Critical" : "Error",
                TraceId = context.TraceIdentifier,
                CreatedAt = DateTime.Now
            };

            db.ErrorLogs.Add(errorLog);
            await db.SaveChangesAsync();

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                statusCode,
                message = ex.Message,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }

    private static int MapStatusCode(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}