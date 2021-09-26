using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Product
{
    public class ProductColorDiscountsComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductColorDiscountsComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageId = 1, string filter = "")
        {
            int take = 20;

           
            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Product/BrandsList.cshtml");
        }
    }
}