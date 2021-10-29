using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using System.Threading.Tasks;

namespace Reshop.Web.Components.User
{
    public class UserOrdersComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public UserOrdersComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int pageId = 1, string type = "all", string orderBy = "news")
        {
            int take = 20;

            var orders = await _cartService.GetUserOrdersForShowWithPaginationAsync(userId,pageId,take,type,orderBy);

            ViewBag.TakeCount = take;
            ViewBag.SelectedType = type;
            ViewBag.SelectedOrderBy = orderBy;

            return View("", orders);
        }
    }
}
