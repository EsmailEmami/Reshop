using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.Entities.User;

namespace Reshop.Web.Components.Product
{
    public class ProductCommentsComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductCommentsComponent(IProductService productService)
        {
            _productService = productService;
        }

        // type = news,buyers,best
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageId = 1, string type = "news")
        {
            var comments = await _productService.GetProductCommentsWithPaginationAsync(productId, pageId, 30, type);

            ViewBag.SelectedType = type;
            ViewBag.ProductId = productId;

            return View("/Views/Shared/Components/Product/ProductComments.cshtml", comments);
        }
    }
}