using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Web.Middleware;

public class AuthorizationErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorDto
            {
                StatusCode = context.Response.StatusCode,
                Message = context.Response.StatusCode == 401 ? "Unauthorized" : "Forbidden"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}