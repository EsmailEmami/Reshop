using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Reshop.Application.Security.Middleware.HitCounter
{
    public class ProductHitCounterMiddleware
    {
        private readonly RequestDelegate _next;

        public ProductHitCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path.StartsWith("/product"))
            {
                string visitorId = context.Request.Cookies["VisitorId"];
                if (visitorId == null)
                {
                    context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                    {
                        Path = path,
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddMinutes(2)
                    });
                }

                await _next(context);
            }
        }
    }
}