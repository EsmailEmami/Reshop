using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;

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

            var model = new AddOrEditShopperProduct()
            {
                ProductId = _dataProtector.Protect(productId.ToString()),
                ShopperId = _dataProtector.Protect(shopperId),
                RequestUserId = _dataProtector.Protect(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperToProduct(AddOrEditShopperProduct model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ShopperId = _dataProtector.Unprotect(model.ShopperId);
                model.ProductId = _dataProtector.Unprotect(model.ProductId);
                model.RequestUserId = _dataProtector.Unprotect(model.RequestUserId);

                // try to convert to string of it is not true means user was changed it
                Convert.ToInt32(model.ProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }



            var shopperProduct = new ShopperProduct()
            {
                ShopperId = model.ShopperId,
                ProductId = Convert.ToInt32(model.ProductId),
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsFinally = true,
                CreateDate = DateTime.Now,
                Warranty = model.Warranty
            };



            var result = await _shopperService.AddShopperProductAsync(shopperProduct);

            if (result == ResultTypes.Successful)
            {
                var shopperProductRequest = new ShopperProductRequest()
                {
                    ShopperId = model.ShopperId,
                    ProductId = Convert.ToInt32(model.ProductId),
                    RequestType = true,
                    RequestDate = DateTime.Now,
                    Warranty = model.Warranty,
                    Price = model.Price,
                    QuantityInStock = model.QuantityInStock,
                    IsSuccess = true,
                    RequestUserId = model.RequestUserId,
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
        public async Task<IActionResult> EditProductOfShopper(int productId, string shopperId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();


            var product = await _productService.GetShopperProductAsync(productId, shopperId);

            if (product is null)
                return NotFound();

            var model = new AddOrEditShopperProduct()
            {
                ShopperId = _dataProtector.Protect(shopperId),
                ProductId = _dataProtector.Protect(productId.ToString()),
                RequestUserId = _dataProtector.Protect(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Warranty = product.Warranty
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(AddOrEditShopperProduct model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ProductId = _dataProtector.Unprotect(model.ProductId);
                model.RequestUserId = _dataProtector.Unprotect(model.RequestUserId);
                model.ShopperId = _dataProtector.Unprotect(model.ShopperId);


                // try to convert to string of it is not true means user was changed it
                Convert.ToInt32(model.ProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }

            var product = await _productService.GetShopperProductAsync(Convert.ToInt32(model.ProductId), model.ShopperId);

            product.IsFinally = model.IsActive;

            await _shopperService.EditShopperProductAsync(product);


            var shopperProductRequest = new ShopperProductRequest()
            {
                ShopperId = model.ShopperId,
                ProductId = Convert.ToInt32(model.ProductId),
                RequestType = false,
                RequestDate = DateTime.Now,
                Warranty = model.Warranty,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = true,
                RequestUserId = model.RequestUserId,
                Reason = ShopperProductRequestReasons.AdminEdited()
            };
            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });
            }
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
