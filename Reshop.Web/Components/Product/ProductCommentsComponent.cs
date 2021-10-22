using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Conversation;

namespace Reshop.Web.Components.Product
{
    public class ProductCommentsComponent : ViewComponent
    {
        private readonly ICommentService _commentService;

        public ProductCommentsComponent(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // type = news,buyers,best
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageId = 1, string type = "news")
        {
            var comments = await _commentService.GetProductCommentsWithPaginationAsync(productId, pageId, 25, type);

            ViewBag.SelectedType = type;
            ViewBag.ProductId = productId;


            List<Tuple<int, bool>> userFeedBacks = new List<Tuple<int, bool>>();
            List<int> reportedComments = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                userFeedBacks = _commentService.GetUserProductCommentsFeedBack(userId, productId).ToList();
                reportedComments = _commentService.GetUserReportCommentsOfProduct(userId, productId).ToList();
            }

            ViewBag.UserFeedBacks = userFeedBacks;
            ViewBag.ReportedComments = reportedComments;


            return View("/Views/Shared/Components/Product/ProductComments.cshtml", comments);
        }
    }
}