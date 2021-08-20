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

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [AutoValidateAntiforgeryToken]
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
        public IActionResult Index(string type = "all", string filter = "", int pageId = 1)
        {
            return View(_shopperService.GetShoppersInformationWithPagination(type, filter, pageId, 24));
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


        /// <summary>
        /// in this method we do not need to validation
        ///  AccountManager/AddShopperToProduct shopper need to validation
        /// </summary>

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperToProduct(int productId, string shopperUserId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(shopperUserId);

            if (shopperId is null)
                return NotFound();

            var model = new AddShopperProductViewModel()
            {
                ProductId = _dataProtector.Protect(productId.ToString()),
                ShopperId = _dataProtector.Protect(shopperId),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperToProduct(AddShopperProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ShopperId = _dataProtector.Unprotect(model.ShopperId);
                model.ProductId = _dataProtector.Unprotect(model.ProductId);

                // try to convert to string of it is not true means user was changed it
                Convert.ToInt32(model.ProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var shopperProduct = new ShopperProduct()
            {
                ShopperId = model.ShopperId,
                ProductId = Convert.ToInt32(model.ProductId),
                IsFinally = true,
                IsActive = model.IsActive,
                CreateDate = DateTime.Now,
                Warranty = model.Warranty
            };


            var result = await _shopperService.AddShopperProductAsync(shopperProduct);

            if (result == ResultTypes.Successful)
            {

                var shopperProductRequest = new ShopperProductRequest()
                {
                    ShopperProductId = shopperProduct.ShopperProductId,
                    ProductId = Convert.ToInt32(model.ProductId),
                    RequestType = true,
                    RequestDate = DateTime.Now,
                    Warranty = model.Warranty,
                    IsSuccess = true,
                    IsRead = true,
                    RequestUserId = userId,
                    Reason = ShopperProductRequestReasons.AdminAdded(),
                };
                await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });
            }
        }



        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(string shopperProductColorId)
        {
            var model = await _productService.GetShopperProductForEditAsync(shopperProductColorId);

            if (model == null)
            {
                return NotFound();
            }


            model.ShopperProductColorId = _dataProtector.Protect(model.ShopperProductColorId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(EditProductOfShopperViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductColor = await _shopperService.GetShopperProductColorAsync(model.ShopperProductColorId);

            if (shopperProductColor is null)
            {
                return NotFound();
            }


            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductColorId = model.ShopperProductColorId,
                RequestType = false,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = true,
                IsRead = true,
                RequestUserId = userId,
                Reason = ShopperProductRequestReasons.AdminEdited()
            };
            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                shopperProductColor.Price = model.Price;
                shopperProductColor.IsActive = model.IsActive;
                shopperProductColor.QuantityInStock = model.QuantityInStock;


                var editShopperProductColor = await _shopperService.EditShopperProductColorAsync(shopperProductColor);

                if (editShopperProductColor == ResultTypes.Successful)
                {
                    return Json(new { isValid = true });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

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
