using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Product
{
    public class PopularProductsComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public PopularProductsComponent(IProductService productService)
        {
            _productService = productService;
        }


        public IViewComponentResult Invoke()
        {
            var products = _productService.GetProductsWithType(ProductTypes.All, SortTypes.MostSale, 20);


            return View("/Views/Shared/Components/Product/PopularProducts.cshtml", products);
        }
    }
}
