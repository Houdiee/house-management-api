using HouseManagementApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HouseManagementApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int status, string title, string detail) = exception switch
        {
            // User
            UserNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            UserAlreadyExistsException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),

            // House
            HouseNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected problem occurred"),
        };

        ProblemDetails problemDetails = new()
        {
            Status = status,
            Title = title,
            Detail = detail,
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
