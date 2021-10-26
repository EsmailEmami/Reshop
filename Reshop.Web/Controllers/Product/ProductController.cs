using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.User;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Comment;

namespace Reshop.Web.Controllers.Product
{
    public class ProductController : Controller
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, ICartService cartService, ICommentService commentService)
        {
            _productService = productService;
            _cartService = cartService;
            _commentService = commentService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Product/{productName}/{sellerId}")]
        public async Task<IActionResult> ProductDetail(string productName, string sellerId)
        {
            if (sellerId == null)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductDetailAsync(sellerId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // change seller 
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ChangeProductShopper(string seller)
        {
            if (string.IsNullOrEmpty(seller))
                return BadRequest();


            var product = await _productService.EditProductDetailShopperAsync(seller);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        [Route("p/{key}")]
        public async Task<IActionResult> ShortKeyRedirect(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();


            var product = await _productService.GetProductRedirectionByShortKeyAsync(key);

            if (product == null)
                return NotFound();


            return RedirectToAction("ProductDetail", "Product",
                new { productName = product.Item1.Replace(" ", "-"), sellerId = product.Item2 });
        }

        [HttpGet]
        [Route("Category/{categoryId}/{categoryName}")]
        public async Task<IActionResult> ProductsOfCategory(int categoryId, string categoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetCategoryProductsWithPaginationAsync(categoryId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);

            ViewData["CategoryName"] = categoryName;

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = minPrice.ToDecimal();
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}")]
        public async Task<IActionResult> ProductsOfChildCategory(int childCategoryId, string childCategoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetChildCategoryProductsWithPaginationAsync(childCategoryId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);


            ViewData["ChildCategoryName"] = childCategoryName;

            if (result == null)
            {

            }

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("Brand/{brandId}/{brandName}")]
        public async Task<IActionResult> ProductsOfBrand(int brandId, string brandName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string officialBrandProducts = null)
        {
            var selectedOfficialBrandProducts = Fixer.SplitToListInt(officialBrandProducts);

            if (selectedOfficialBrandProducts == null)
                return NotFound();

            var result = await _productService.GetBrandProductsWithPaginationAsync(brandId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedOfficialBrandProducts);

            ViewData["BrandName"] = brandName;


            ViewBag.SortBy = sortBy;
            ViewBag.SelectedOfficialBrandProducts = selectedOfficialBrandProducts;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs]
        public async Task<IActionResult> AddToFavoriteProduct(string shopperProductColorId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _productService.AddFavoriteProductAsync(userId, shopperProductColorId);

            return result switch
            {
                FavoriteProductResultType.Successful => Json(new { success = true, resultType = "Successful" }),
                FavoriteProductResultType.ProductReplaced => Json(new { success = true, resultType = "ProductReplaced" }),
                FavoriteProductResultType.NotFound => Json(new { success = false, resultType = "NotFound" }),
                FavoriteProductResultType.Available => Json(new { success = false, resultType = "Available" }),
                _ => Json(new { success = false, resultType = "NotFound" })
            };
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteProduct(string favoriteProductId)
        {
            var favoriteProduct = await _productService.GetFavoriteProductByIdAsync(favoriteProductId);
            if (favoriteProduct != null)
            {
                await _productService.RemoveFavoriteProductAsync(favoriteProduct);
                return Redirect("/");
            }
            else
            {
                return NotFound();
            }
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


            var comment = new Comment()
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


            var model = new AddReportCommentViewModel()
            {
                CommentId = commentId,
                Types = _commentService.GetReportCommentTypes()
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> ReportComment(AddReportCommentViewModel model)
        {
            model.Types = _commentService.GetReportCommentTypes();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _commentService.ReportCommentByUserAsync(userId, model);

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
