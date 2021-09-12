using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Shopper
{
    public class ShopperProductsOfShopperComponent : ViewComponent
    {
        private readonly IShopperService _shopperService;

        public ShopperProductsOfShopperComponent(IShopperService shopperService)
        {
            _shopperService = shopperService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string shopperId, string type = "all", int pageId = 1, string filter = "")
        {
            int take = 20;

            var products = await _shopperService.GetShopperProductsInformationWithPagination(shopperId, type, filter, pageId, take);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;
            ViewBag.ShopperId = shopperId;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Shopper/ShopperProductsOfShopper.cshtml", products);
        }
    }
}
