using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;

namespace Reshop.Web.Components.Shopper
{
    public class ShoppersListOfProductComponent : ViewComponent
    {
        private readonly IShopperService _shopperService;

        public ShoppersListOfProductComponent(IShopperService shopperService)
        {
            _shopperService = shopperService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId, string type = "all", int pageId = 1, int take = 50, string filter = "")
        {
            var products = await _shopperService.GetProductShoppersInformationWithPagination(productId, type, filter, pageId, 35);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;

            return View("/Views/Shared/Components/Shopper/ShoppersListOfProduct.cshtml", products);
        }
    }
}