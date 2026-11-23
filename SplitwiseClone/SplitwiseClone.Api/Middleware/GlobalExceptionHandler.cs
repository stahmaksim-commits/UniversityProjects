using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SplitwiseClone.Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception on {Path}", httpContext.Request.Path);

        int statusCode = StatusCodes.Status500InternalServerError;
        string title = "Internal server error";

        if (exception is KeyNotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
            title = "Not found";
        }
        else if (exception is ArgumentException or InvalidOperationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            title = "Bad request";
        }
        else if (exception is UnauthorizedAccessException)
        {
            statusCode = StatusCodes.Status403Forbidden;
            title = "Forbidden";
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError ? "Unexpected error." : exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}