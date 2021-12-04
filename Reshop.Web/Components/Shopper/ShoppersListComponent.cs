using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;

namespace Reshop.Web.Components.Shopper
{
    public class ShoppersListComponent : ViewComponent
    {
        private readonly IShopperService _shopperService;

        public ShoppersListComponent(IShopperService shopperService)
        {
            _shopperService = shopperService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type = "all", int pageId = 1, string filter = "")
        {
            int take = 1;

            var products = await _shopperService.GetShoppersInformationWithPagination(type, filter, pageId, take);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Shopper/ShoppersList.cshtml", products);
        }
    }
}