using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.Product
{
    public class ProductColorDiscountsComponent : ViewComponent
    {
        private readonly IDiscountService _discountService;

        public ProductColorDiscountsComponent(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId, int colorId, int pageId = 1, string filter = "")
        {
            int take = 20;

            var data = await _discountService.GetProductColorDiscountsWithPaginationAsync(productId, colorId, pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;

            return View("/Views/Shared/Components/Product/ProductColorDiscounts.cshtml", data);
        }
    }
}