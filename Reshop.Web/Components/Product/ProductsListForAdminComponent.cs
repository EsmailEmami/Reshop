using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Product
{
    public class ProductsListForAdminComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsListForAdminComponent(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string type = "all", int pageId = 1, string filter = "")
        {
            var products = await _productService.GetProductsWithPaginationForAdminAsync(type, pageId, 50, filter);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;

            return View("/Views/Shared/Components/Product/ProductsListForAdmin.cshtml", products);
        }
    }
}
