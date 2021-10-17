using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Web.Components.Product
{
    public class ProductCommentsComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductCommentsComponent(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        // type = news,buyers,best
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageId = 1, string type = "news")
        {
            var comments = await _productService.GetProductCommentsWithPaginationAsync(productId, pageId, 1, type);

            ViewBag.SelectedType = type;
            ViewBag.ProductId = productId;


            List<Tuple<int, bool>> userFeedBacks = new List<Tuple<int, bool>>();

            if (User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                userFeedBacks = _userService.GetUserProductCommentsFeedBack(userId, productId).ToList();
            }

            ViewBag.UserFeedBacks = userFeedBacks;


            return View("/Views/Shared/Components/Product/ProductComments.cshtml", comments);
        }
    }
}