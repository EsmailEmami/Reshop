using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Comment;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers.Comment
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICartService _cartService;

        public CommentController(ICommentService commentService, ICartService cartService)
        {
            _commentService = commentService;
            _cartService = cartService;
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult ProductCommentsList(int productId, int pageId, string type)
        {
            return ViewComponent("ProductCommentsComponent", new { productId, pageId, type });
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs]
        public async Task<IActionResult> AddComment(NewCommentViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "_NewComment", model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            string isBought = await _cartService.IsUserBoughtProductAsync(userId, model.ProductId);


            var average = (model.ProductSatisfaction +
                           model.ConstructionQuality +
                           model.FeaturesAndCapabilities +
                           model.DesignAndAppearance) / 4;


            var comment = new Domain.Entities.Comment.Comment()
            {
                ProductId = model.ProductId,
                UserId = userId,
                CommentDate = DateTime.Now,
                CommentTitle = model.CommentTitle,
                CommentText = model.CommentText,
                ProductSatisfaction = model.ProductSatisfaction,
                DesignAndAppearance = model.DesignAndAppearance,
                ConstructionQuality = model.ConstructionQuality,
                FeaturesAndCapabilities = model.FeaturesAndCapabilities,
                OverallScore = average,
                IsDelete = false
            };

            if (!string.IsNullOrEmpty(isBought))
            {
                comment.ShopperProductColorId = isBought;
            }

            var res = await _commentService.AddCommentAsync(comment);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام افزودن بازخورد شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "_NewComment", model) });
            }
        }

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> ReportComment(int commentId)
        {
            if (commentId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _commentService.IsReportCommentTimeLockAsync(userId, commentId))
                return Json(new { isValid = false, errorType = "warning", errorText = "لطفا چند دقیقه دیگر تلاش کنید." });


            var model = new AddReportConversationViewModel()
            {
                Id = commentId,
                Types = _commentService.GetReportCommentTypes()
                    .Select(c => new Tuple<int, string>(
                        c.ReportCommentTypeId, 
                        c.ReportCommentTitle))
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> ReportComment(AddReportConversationViewModel model)
        {
            model.Types = _commentService.GetReportCommentTypes().Select(c => new Tuple<int, string>(c.ReportCommentTypeId, c.ReportCommentTitle));

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reportComment = new ReportComment()
            {
                UserId = userId,
                CommentId = model.Id,
                Description = model.Description,
                ReportCommentTypeId = model.SelectedType,
                CreateDate = DateTime.Now
            };


            var res = await _commentService.AddReportCommentAsync(reportComment);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> RemoveCommentFromReport(int commentId)
        {
            if (commentId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _commentService.IsReportCommentTimeLockAsync(userId, commentId))
                return Json(new { isValid = false, errorType = "warning", errorText = "لطفا چند دقیقه دیگر تلاش کنید." });


            var res = await _commentService.RemoveReportCommentByUserAsync(userId, commentId);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> LikeOrDisLikeComment(int commentId, string type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _commentService.AddCommentFeedBackAsync(userId, commentId, type);

            return res switch
            {
                CommentFeedBackType.LikeAdded => Json(new
                {
                    isValid = true,
                    where = "LikeAdded",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت ثبت شد."
                }),
                CommentFeedBackType.DislikeAdded => Json(new
                {
                    isValid = true,
                    where = "DislikeAdded",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت ثبت شد."
                }),
                CommentFeedBackType.LikeRemoved => Json(new
                {
                    isValid = true,
                    where = "LikeRemoved",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت حذف شد."
                }),
                CommentFeedBackType.DislikeRemoved => Json(new
                {
                    isValid = true,
                    where = "DislikeRemoved",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت حذف شد."
                }),
                CommentFeedBackType.LikeEdited => Json(new
                {
                    isValid = true,
                    where = "LikeEdited",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت تغییر یافت."
                }),
                CommentFeedBackType.DislikeEdited => Json(new
                {
                    isValid = true,
                    where = "DislikeEdited",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت تغییر یافت."
                }),
                CommentFeedBackType.Error => Json(new
                {
                    isValid = false,
                    errorType = "danger",
                    errorText = "متاسفانه مشکلی پیش آمده است."
                }),
                _ => Json(new { isValid = false, errorType = "danger", errorText = "متاسفانه مشکلی پیش آمده است." })
            };
        }
    }
}
