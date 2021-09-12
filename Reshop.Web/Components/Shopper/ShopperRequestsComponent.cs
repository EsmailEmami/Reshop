using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;

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
        public async Task<IViewComponentResult> InvokeAsync(string shopperId, string type = "all", int pageId = 1)
        {
            int take = 20;

            var requests = await _shopperService.GetShopperRequestsForShowAsync(shopperId, type, pageId, take);

            ViewBag.SelectedType = type;
            ViewBag.ShopperId = shopperId;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Shopper/ShopperRequests.cshtml", requests);
        }
    }
}
