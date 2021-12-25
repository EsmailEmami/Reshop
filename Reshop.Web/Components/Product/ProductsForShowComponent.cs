using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Product
{
    public class ProductsForShowComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsForShowComponent(IProductService productService)
        {
            _productService = productService;
        }


        public IViewComponentResult Invoke(string type, string sortBy, int take = 24, string brands = null)
        {
            if (brands != null && brands.ToLower() == "null")
                brands = null;

            var selectedBrands = Fixer.SplitToListInt(brands);

            var products = _productService.GetProductsForShow(type, sortBy, take, selectedBrands);

            ViewBag.SelectedType = type;

            return View("/Views/Shared/Components/Product/ProductsForShow.cshtml", products);
        }
    }
}
