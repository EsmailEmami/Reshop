using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Domain.Entities.Product;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Product;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class BrandManagerController : Controller
    {
        private readonly IShopperService _shopperService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public BrandManagerController(IShopperService shopperService, IBrandService brandService, ICategoryService categoryService)
        {
            _shopperService = shopperService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult BrandsList(int pageId = 1, string filter = "")
        {
            return ViewComponent("BrandsListComponent", new { pageId, filter });
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult BrandDetail(int brandId)
        {
            if (brandId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = _brandService.GetBrandOfficialProducts(brandId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            return View(model);
        }


        #region add brand

        [HttpGet]
        [NoDirectAccess]
        public IActionResult AddBrand()
        {
            var model = new AddOrEditBrandViewModel()
            {
                StoreTitles = _shopperService.GetStoreTitles(),
                ChildCategories = _categoryService.GetChildCategories()
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddBrand(AddOrEditBrandViewModel model)
        {
            model.StoreTitles = _shopperService.GetStoreTitles();
            model.ChildCategories = _categoryService.GetChildCategories();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var brand = new Brand()
            {
                BrandName = model.BrandName,
                LatinBrandName = model.LatinBrandName,
                StoreTitleId = model.SelectedStoreTitleId,
                IsActive = model.IsActive
            };

            var addBrand = await _brandService.AddBrandAsync(brand);

            if (addBrand == ResultTypes.Successful)
            {
                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    var addBrandToChildCategory = await _brandService.AddBrandToChildCategoriesByBrandAsync(brand.BrandId, model.SelectedChildCategories as List<int>);

                    if (addBrandToChildCategory == ResultTypes.Successful)
                    {
                        return Json(new { isValid = true, returnUrl = "current" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "هنگام افزودن برند به مشکل خوردیم");
                        return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
                    }
                }
                else
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }
            }
            ModelState.AddModelError("", "هنگام افزودن برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        #endregion

        #region edit brand

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditBrand(int brandId)
        {
            if (brandId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var brand = await _brandService.GetBrandDataByBrandIdAsync(brandId);

            if (brand == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(brand);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditBrand(AddOrEditBrandViewModel model)
        {
            model.StoreTitles = _shopperService.GetStoreTitles();
            model.ChildCategories = _categoryService.GetChildCategories();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var brand = await _brandService.GetBrandByIdAsync(model.BrandId);

            brand.BrandName = model.BrandName;
            brand.LatinBrandName = model.LatinBrandName;
            brand.IsActive = model.IsActive;
            brand.StoreTitleId = model.SelectedStoreTitleId;

            var editBrand = await _brandService.EditBrandAsync(brand);


            if (editBrand == ResultTypes.Successful)
            {
                await _brandService.RemoveBrandToChildCategoriesByBrandAsync(model.BrandId);


                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    var addBrandToChildCategory = await _brandService.AddBrandToChildCategoriesByBrandAsync(model.BrandId, model.SelectedChildCategories as List<int>);

                    if (addBrandToChildCategory == ResultTypes.Successful)
                    {
                        return Json(new { isValid = true, returnUrl = "current" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "هنگام افزودن برند به مشکل خوردیم");
                        return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
                    }
                }
                else
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }
            }

            ModelState.AddModelError("", "هنگام ویرایش برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        #endregion

        #region Available

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AvailableBrand(int brandId)
        {
            if (!await _brandService.IsBrandExistAsync(brandId))
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }

            return View(new AvailableBrandViewModel() { BrandId = brandId });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AvailableBrand(AvailableBrandViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            var res = await _brandService.AvailableBrand(model);


            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "هنگام فعال شدن برند به مشکل خوردیم");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> UnAvailableBrand(int brandId)
        {
            var res = await _brandService.UnAvailableBrand(brandId);

            if (res != ResultTypes.Successful)
                return BadRequest();


            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
