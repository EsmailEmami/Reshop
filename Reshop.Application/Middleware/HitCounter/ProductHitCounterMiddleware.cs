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
                    int productId = int.Parse(path.TextAfter("/product/").Split("/").First());
                    if (await _productService.IsProductExistAsync(productId))
                    {
                        string userIP = context.Connection.RemoteIpAddress.ToString();
                        if (!await _productService.IsUserProductViewExistAsync(productId, userIP))
                        {
                            var userProductView = new UserProductView()
                            {
                                UserIPAddress = userIP,
                                ProductId = productId,
                            };

                            var result = await _productService.AddUserProductViewAsync(userProductView);

                            if (result == ResultTypes.Successful)
                            {
                                var product = await _productService.GetProductByIdAsync(productId);
                                //product.AllViewsCount += 1;

                                await _productService.EditProductAsync(product);
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
