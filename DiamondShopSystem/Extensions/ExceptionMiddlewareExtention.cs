using DiamondShopSystem.Middleware;

namespace DiamondShopSystem.Extensions
{
    public static class ExceptionMiddlewareExtention
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
