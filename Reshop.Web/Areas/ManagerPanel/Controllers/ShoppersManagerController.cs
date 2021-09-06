using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class ShoppersManager : Controller
    {
        private readonly IShopperService _shopperService;
        private readonly IProductService _productService;
        private readonly IDataProtector _dataProtector;

        public ShoppersManager(IShopperService shopperService, IProductService productService, IDataProtectionProvider dataProtectionProvider)
        {
            _shopperService = shopperService;
            _productService = productService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Areas.ManagerPanel.Controllers.ShoppersManager",
                new string[] { "ShoppersManager" });
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // this method is for changing page or filter of shoppers list
        [HttpGet]
        [NoDirectAccess]
        public IActionResult ShoppersList(string type, int pageId, string filter)
        {
            return ViewComponent("ShoppersListComponent", new { type, pageId, take = 50, filter });
        }

        [HttpGet]
        public async Task<IActionResult> ShopperDetail(string shopperId)
        {
            if (shopperId == null) return NotFound();

            var model = await _shopperService.GetShopperDataForAdminAsync(shopperId);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShopperProductDetail(string shopperProductId)
        {
            if (shopperProductId == null)
                return NotFound();

            var model = await _productService.GetProductDetailForShopperAsync(shopperProductId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // in this method we create new shopper product for shopper 
        // we do not need validation product in this method
        // this method open in modal
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProduct(string shopperId)
        {
            if (shopperId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (!await _shopperService.IsShopperExistAsync(shopperId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده یافت نشد! لطفا دوباره تلاش کنید." });

            var model = new AddOrEditShopperProductViewModel()
            {
                //ShopperId = _dataProtector.Protect(shopperId),

                ShopperId = shopperId,
                StoreTitles = _shopperService.GetShopperStoreTitles(shopperId),
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShopperProduct(AddOrEditShopperProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperProduct", model) });

            //try
            //{
            //    model.ShopperId = _dataProtector.Unprotect(model.ShopperId);

            //    Guid.Parse(model.ShopperId);
            //}
            //catch
            //{
            //    ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            //    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperProduct", model) });
            //}


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductRequest = new ShopperProductRequest()
            {
                ShopperId = model.ShopperId,
                ProductId = model.ProductId,
                RequestUserId = userId,
                RequestType = true,
                Warranty = model.Warranty,
                RequestDate = DateTime.Now,
                IsSuccess = true,
                Reason = ShopperProductRequestReasons.AdminAdded(),
                IsRead = true,
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                var shopperProduct = new ShopperProduct()
                {
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    IsFinally = true,
                    ProductId = model.ProductId,
                    ShopperId = model.ShopperId,
                    Warranty = model.Warranty
                };

                var addShopperProduct = await _shopperService.AddShopperProductAsync(shopperProduct);

                if (addShopperProduct == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperProduct", model) });
        }

        [HttpGet]
        public IActionResult ManageStoreTitles()
        {
            return View(_shopperService.GetStoreTitles());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditStoreTitle(int storeTitleId = 0)
        {
            if (storeTitleId == 0)
            {
                return View(new StoreTitle() { StoreTitleId = 0 });
            }
            else
            {
                var storeTitle = await _shopperService.GetStoreTitleByIdAsync(storeTitleId);
                return View(storeTitle);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditStoreTitle(StoreTitle model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.StoreTitleId == 0)
            {
                var storeTilte = new StoreTitle()
                {
                    StoreTitleName = model.StoreTitleName
                };

                var result = await _shopperService.AddStoreTitleAsync(storeTilte);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(ManageStoreTitles));
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام افزودن عنوان به مشکل برخوردیم.");
                return View(model);
            }
            else
            {
                var storeTitle = await _shopperService.GetStoreTitleByIdAsync(model.StoreTitleId);

                if (storeTitle == null) return NotFound();


                storeTitle.StoreTitleName = model.StoreTitleName;

                var result = await _shopperService.EditStoreTitleAsync(storeTitle);

                if (result == ResultTypes.Successful)
                {
                    return RedirectToAction(nameof(ManageStoreTitles));
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش عنوان به مشکل برخوردیم.");
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStoreTitle(int storeTitleId)
        {
            await _shopperService.DeleteStoreTitleAsync(storeTitleId);


            return RedirectToAction(nameof(ManageStoreTitles));
        }


     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> UnAvailableShopperProduct(string shopperProductId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
            {
                return Json(new { isValid = false });
            }


            var shopperProduct = await _productService.GetShopperProductAsync(shopperProductId);

            if (shopperProduct is null)
            {
                return Json(new { isValid = false });
            }

            shopperProduct.IsFinally = false;

            var result = await _shopperService.EditShopperProductAsync(shopperProduct);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false });
            }
        }
    }
}
