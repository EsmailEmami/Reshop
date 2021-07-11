using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.Entities.Product;

namespace Reshop.Application.Middleware.HitCounter
{
    public class ProductHitCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private IProductService _productService;

        public ProductHitCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _productService = (IProductService)context.RequestServices.GetService(typeof(IProductService));
            string path = context.Request.Path.Value.ToLower();
            string refererPath = context.Request.Headers["Referer"].ToString().ToLower();

            if (path.StartsWith("/product") && !refererPath.StartsWith("https://localhost:44312/product"))
            {
                if (_productService != null)
                {
                    try
                    {
                        int productId = int.Parse(path.TextAfter("/product/").Split("/").First());

                        if (await _productService.IsProductExistAsync(productId))
                        {

                        }
                    }
                    catch
                    {
                        await _next(context);
                    }
                }
            }
            await _next(context);
        }
    }
}
