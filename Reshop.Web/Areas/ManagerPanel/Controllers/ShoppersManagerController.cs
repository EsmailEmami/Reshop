using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
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
        public IActionResult Index(int pageId = 1, int take = 24)
        {
            return View(/*_shopperService.GetShoppersInformationWithPagination(pageId, take)*/);
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
        public async Task<IActionResult> AddShopperToProduct(int productId, string shopperUserId )
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            if (!await _shopperService.IsShopperExistAsync(shopperUserId))
                return NotFound();

            var model = new AddOrEditShopperProduct()
            {
                ProductId = _dataProtector.Protect(productId.ToString())و
                ShopperUserId = _dataProtector.Protect(shopperUserId)
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
                model.ShopperUserId = _dataProtector.Unprotect(model.ShopperUserId);
                model.ProductId = _dataProtector.Unprotect(model.ProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }



            var shopperProduct = new ShopperProduct()
            {
                ShopperUserId = model.ShopperUserId,
                ProductId = Convert.ToInt32(model.ProductId),
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsFinally = true
            };



            var result = await _shopperService.AddShopperProductAsync(shopperProduct);

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



        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(int productId, string shopperUserId)
        {
            if (!await _productService.IsProductExistAsync(productId))
                return NotFound();

            var model = new EditShopperProduct()
            {
               
            };


            // we do not check that user is shopper or no because coming to this method need shopper permission
            if (!string.IsNullOrEmpty(shopperUserId))
            {
                model.ShopperUserId = _dataProtector.Protect(shopperUserId);
            }



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
                if (model.ShopperUserId is not null)
                {
                    model.ShopperUserId = _dataProtector.Unprotect(model.ShopperUserId);
                }

                model.ProductId = _dataProtector.Unprotect(model.ProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت فروشنده به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", null) });
            }



            var shopperProduct = new ShopperProduct()
            {
                ProductId = Convert.ToInt32(model.ProductId),
                Price = model.Price,
                QuantityInStock = model.QuantityInStock
            };

            if (model.ShopperUserId is null)
            {
                shopperProduct.ShopperUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                shopperProduct.IsFinally = false;
            }
            else
            {
                shopperProduct.ShopperUserId = model.ShopperUserId;
                shopperProduct.IsFinally = true;
            }


            var result = await _shopperService.AddShopperProductAsync(shopperProduct);

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
    }
}
