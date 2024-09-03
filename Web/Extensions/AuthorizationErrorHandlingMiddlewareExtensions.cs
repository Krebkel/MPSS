using Microsoft.AspNetCore.Builder;
using Web.Middleware;

namespace Web.Extensions;

public static class AuthorizationErrorHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseAuthorizationErrorHandling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationErrorHandlingMiddleware>();
    }
}