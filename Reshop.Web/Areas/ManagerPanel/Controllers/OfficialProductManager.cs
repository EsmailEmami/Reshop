using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class OfficialProductManager : Controller
    {
        private readonly IBrandService _brandService;

        public OfficialProductManager(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult OfficialBrandProductList(int pageId = 1, string filter = "")
        {
            return ViewComponent("OfficialBrandProductsComponent", new { pageId, filter });
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult OfficialBrandProductDetail(int officialBrandProductId)
        {
            if (officialBrandProductId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = _brandService.GetProductsOfOfficialProduct(officialBrandProductId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            return View(model);
        }

        #region add official product

        [HttpGet]
        [NoDirectAccess]
        public IActionResult AddOfficialBrandProduct()
        {
            ViewBag.Brands = _brandService.GetBrandsForShow();

            return View(new OfficialBrandProduct());
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddOfficialBrandProduct(OfficialBrandProduct model)
        {
            ViewBag.Brands = _brandService.GetBrandsForShow();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var officialBrandProduct = new OfficialBrandProduct()
            {
                OfficialBrandProductName = model.OfficialBrandProductName,
                LatinOfficialBrandProductName = model.LatinOfficialBrandProductName,
                BrandId = model.BrandId,
                IsActive = model.IsActive
            };

            var addBrand = await _brandService.AddOfficialBrandProductAsync(officialBrandProduct);

            if (addBrand == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام افزودن برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        #endregion

        #region edit brand

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditOfficialBrandProduct(int officialBrandProductId)
        {
            if (officialBrandProductId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var officialBrandProduct = await _brandService.GetOfficialBrandProductByIdAsync(officialBrandProductId);

            if (officialBrandProduct == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            ViewBag.Brands = _brandService.GetBrandsForShow();

            return View(officialBrandProduct);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditOfficialBrandProduct(OfficialBrandProduct model)
        {
            ViewBag.Brands = _brandService.GetBrandsForShow();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var officialBrandProduct = await _brandService.GetOfficialBrandProductByIdAsync(model.OfficialBrandProductId);

            officialBrandProduct.OfficialBrandProductName = model.OfficialBrandProductName;
            officialBrandProduct.LatinOfficialBrandProductName = model.LatinOfficialBrandProductName;
            officialBrandProduct.IsActive = model.IsActive;
            officialBrandProduct.BrandId = model.BrandId;

            var editBrand = await _brandService.EditOfficialBrandProductAsync(officialBrandProduct);


            if (editBrand == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام ویرایش نام اختصاصی کالا به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        #endregion

        #region Available

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AvailableOfficialBrandProduct(int officialBrandProductId)
        {
            if (!await _brandService.IsOfficialBrandProductExistAsync(officialBrandProductId))
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }

            return View(new AvailableOfficialBrandProductViewModel());
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AvailableOfficialBrandProduct(AvailableOfficialBrandProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var res = await _brandService.AvailableOfficialBrandProductAsync(model.OfficialBrandProductId, model.AvailableProducts);


            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام فعال شدن برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> UnAvailableOfficialBrandProduct(int officialBrandProductId)
        {
            var res = await _brandService.UnAvailableOfficialBrandProductAsync(officialBrandProductId);

            if (res != ResultTypes.Successful)
                return BadRequest();


            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
