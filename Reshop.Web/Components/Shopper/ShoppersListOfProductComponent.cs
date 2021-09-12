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

        public async Task<IViewComponentResult> InvokeAsync(int productId, string type = "all", int pageId = 1, string filter = "")
        {
            int take = 20;

            var products = await _shopperService.GetProductShoppersInformationWithPagination(productId, type, filter, pageId, take);

            ViewBag.SearchText = filter;
            ViewBag.SelectedType = type;
            ViewBag.TakeCount = take;


            return View("/Views/Shared/Components/Shopper/ShoppersListOfProduct.cshtml", products);
        }
    }
}