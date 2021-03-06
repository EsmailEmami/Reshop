using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Shopper
{
    public class ShopperRequestsComponent : ViewComponent
    {
        private readonly IShopperService _shopperService;

        public ShopperRequestsComponent(IShopperService shopperService)
        {
            _shopperService = shopperService;
        }


        // type = all,product,color
        public async Task<IViewComponentResult> InvokeAsync(string shopperId, string type = "all", int pageId = 1, string filter = null)
        {
            int take = 20;

            var requests = await _shopperService.GetShopperRequestsForShowAsync(shopperId, type, pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;
            ViewBag.ShopperId = shopperId;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Shopper/ShopperRequests.cshtml", requests);
        }
    }
}
