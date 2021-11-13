using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Shopper
{
    public class ShopperProductsForShowShopperComponent : ViewComponent
    {
        private readonly IShopperService _shopperService;

        public ShopperProductsForShowShopperComponent(IShopperService shopperService)
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

            return View("/Views/Shared/Components/Shopper/ShopperProductsForShowShopper.cshtml", products);
        }
    }
}
