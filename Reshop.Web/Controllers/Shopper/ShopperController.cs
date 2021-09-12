using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Web.Controllers.Shopper
{
    [Permission("Shopper")]
    public class ShopperController : Controller
    {
        #region Constructor

        private readonly IProductService _productService;
        private readonly IShopperService _shopperService;
        private readonly IDataProtector _dataProtector;

        public ShopperController(IProductService productService, IShopperService shopperService, IDataProtectionProvider dataProtectionProvider)
        {
            _productService = productService;
            _shopperService = shopperService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Controllers.Shopper.ShopperController",
                new string[] { "ShopperController" });
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> ManageProducts(string type = "all", string sortBy = "news", int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);
            return View(await _productService.GetShopperProductsWithPaginationAsync(shopperId, type, sortBy, pageId, 20));
        }

        [HttpGet]
        public async Task<IActionResult> StoreAddress()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _shopperService.GetShopperStoreAddressesAsync(userId));
        }

        [HttpGet]
        public async Task<IActionResult> ProductsAccess()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId is null)
                return NotFound();

            return View(_shopperService.GetShopperStoreTitlesName(shopperId));
        }

        [HttpGet]
        public async Task<IActionResult> ShopperProductDetail(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId == null)
            {
                return NotFound();
            }

            return View(await _productService.GetProductDetailForShopperAsync(productId, shopperId));
        }
      
        // color
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductColorDetail(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0) return NotFound();


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return NotFound();

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
                return NotFound();

            var model = await _shopperService.GetShopperProductColorDetailAsync(shopperProductColorId);

            if (model is null)
                return NotFound();

            return View(model);
        }

        // discount
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductDiscountDetail(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0) return NotFound();


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return NotFound();

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
                return NotFound();

            var model = await _shopperService.GetShopperProductColorDiscountDetailAsync(shopperProductColorId);

            if (model is null)
                return NotFound();

            return View(model);
        }

        // add color request
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductOfShopper(int productId)
        {
            if (productId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (shopperId is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperProductId = await _shopperService.GetShopperProductIdAsync(shopperId, productId);
            if (shopperProductId is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });



            var model = new AddColorToShopperProductViewModel()
            {
                ShopperProductId = _dataProtector.Protect(shopperProductId)
            };

            ViewData["Colors"] = _shopperService.GetColors();

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductOfShopper(AddColorToShopperProductViewModel model)
        {
            ViewData["Colors"] = _shopperService.GetColors();
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductOfShopper", model) });

            try
            {
                model.ShopperProductId = _dataProtector.Unprotect(model.ShopperProductId);

                Guid.Parse(model.ShopperProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = model.ShopperProductId,
                RequestType = true,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = false,
                IsRead = false,
                RequestUserId = userId,
                ColorId = model.ColorId,
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "" });
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductOfShopper", model) });
        }

        // edit color information
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperProductId = await _shopperService.GetShopperProductIdAsync(shopperId, productId);
            if (shopperProductId is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            if (await _shopperService.IsAnyActiveShopperProductColorRequestAsync(shopperProductId, colorId))
                return Json(new { isValid = false, errorType = "warning", errorText = "درخواست قبلی شما بررسی نشده است. لطفا منتظر بمانید." });

            var model = await _productService.GetShopperProductColorForEditAsync(productId, shopperProductId, colorId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            model.ShopperProductColorId = _dataProtector.Protect(model.ShopperProductColorId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(EditColorOfShopperProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);

                Guid.Parse(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var shopperProductColor = await _shopperService.GetShopperProductColorAsync(model.ShopperProductColorId);
            if (shopperProductColor is null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
            }

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = shopperProductColor.ShopperProductId,
                RequestType = false,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = false,
                IsRead = false,
                RequestUserId = userId,
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                shopperProductColor.IsActive = model.IsActive;

                var edit = await _shopperService.EditShopperProductColorAsync(shopperProductColor);

                if (edit == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "" });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
        }

        // new discount
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });


            if (await _shopperService.IsActiveShopperProductColorDiscountExistsAsync(shopperProductColorId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده عزیز شما یک تخفیف فعال دارید." });

            var model = new AddOrEditShopperProductDiscountViewModel()
            {
                ShopperProductColorId = _dataProtector.Protect(shopperProductColorId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(AddOrEditShopperProductDiscountViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت تخفیف به مشتکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
            }

            DateTime startDate = model.StartDate.ConvertPersianDateTimeToEnglishDateTime();
            DateTime endDate = model.EndDate.ConvertPersianDateTimeToEnglishDateTime();



            if (startDate >= endDate || endDate.Subtract(startDate) < TimeSpan.FromHours(12) ||
                startDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
            }



            var shopperProductDiscount = new ShopperProductDiscount()
            {
                ShopperProductColorId = model.ShopperProductColorId,
                StartDate = startDate,
                EndDate = endDate,
                DiscountPercent = model.DiscountPercent,
            };

            var result = await _shopperService.AddShopperProductDiscountAsync(shopperProductDiscount);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "" });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnAvailableProduct(int productId)
        {
            if (productId == 0)
            {
                return Json(new { isValid = false });
            }


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
            {
                return Json(new { isValid = false });
            }

            var result = await _shopperService.UnAvailableShopperProductAsync(shopperId, productId);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> ShopperRequests(int pageId = 1)
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

        //    if (shopperId == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = await _shopperService.GetShopperRequestsForShowAsync(shopperId,ty, 24);

        //    return View(model);
        //}
    }
}