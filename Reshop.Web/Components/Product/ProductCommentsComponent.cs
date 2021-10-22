using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var comments = await _productService.GetProductCommentsWithPaginationAsync(productId, pageId, 25, type);

            ViewBag.SelectedType = type;
            ViewBag.ProductId = productId;


            List<Tuple<int, bool>> userFeedBacks = new List<Tuple<int, bool>>();
            List<int> ReportedComments = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                userFeedBacks = _userService.GetUserProductCommentsFeedBack(userId, productId).ToList();
                ReportedComments = _userService.GetUserReportCommentsOfProduct(userId, productId).ToList();
            }

            ViewBag.UserFeedBacks = userFeedBacks;
            ViewBag.ReportedComments = ReportedComments;


            return View("/Views/Shared/Components/Product/ProductComments.cshtml", comments);
        }
    }
}