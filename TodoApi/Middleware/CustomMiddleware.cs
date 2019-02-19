using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// Custom middleware, read more at:
// https://adamstorr.azurewebsites.net/blog/aspnetcore-exploring-custom-middleware

namespace TodoApi.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CustomMiddlewareOptions _options;

        public CustomMiddleware(RequestDelegate next, CustomMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_options.DisplayBefore)
            {
                Console.WriteLine("------- Custom Middleware Before ------ \n\r");
            }

            await _next(context);

            if (_options.DisplayAfter)
            {
                Console.WriteLine("\n\r------- Custom Middleware After ------");
            }
        }
    }
}
