using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Shopper
{
    public class ShoppersListOfProductComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ShoppersListOfProductComponent(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string type = "all", int pageId = 1, int take = 50, string filter = "")
        {
            var products = "";
            return View("/Views/Shared/Components/Product/ProductsListForAdmin.cshtml", products);
        }
    }
}