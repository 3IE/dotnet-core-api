using Microsoft.AspNetCore.Builder;

namespace TodoApi.Middleware
{
    public static class CustomMiddlewareWithOptionsMultiUseExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder, CustomMiddlewareOptions options)
        {
            options = options ?? new CustomMiddlewareOptions();
            return builder.UseMiddleware<CustomMiddleware>(options);
        }
    }
}
