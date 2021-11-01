using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class ChildCategoryManagerController : Controller
    {
        #region constructor

        private readonly ICategoryService _categoryService;

        public ChildCategoryManagerController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_categoryService.GetChildCategories());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditChildCategory(int childCategoryId = 0)
        {
            if (childCategoryId == 0)
            {
                var model = new AddOrEditChildCategoryViewModel()
                {
                    Categories = _categoryService.GetCategories()
                };

                return View(model);
            }
            else
            {
                var childCategory = await _categoryService.GetChildCategoryDataAsync(childCategoryId);

                if (childCategory is null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


                return View(childCategory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditChildCategory(AddOrEditChildCategoryViewModel model)
        {
            model.Categories = _categoryService.GetCategories();
            if (!ModelState.IsValid)
                return Json(new
                {
                    isValid = false, 
                    html = RenderViewToString.RenderRazorViewToString(this, model)
                });

            if (model.ChildCategoryId == 0)
            {
                var childCategory = new ChildCategory()
                {
                    ChildCategoryTitle = model.ChildCategoryTitle,
                    IsActive = model.IsActive,
                    CategoryId = model.SelectedCategory
                };

                var res = await _categoryService.AddChildCategoryAsync(childCategory);

                if (res != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام افزودن زیر گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }
            }
            else
            {
                var childCategory = await _categoryService.GetChildCategoryByIdAsync(model.ChildCategoryId);

                if (childCategory == null)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش زیر گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }

                // update category
                childCategory.ChildCategoryTitle = model.ChildCategoryTitle;
                childCategory.IsActive = model.IsActive;
                childCategory.CategoryId = model.SelectedCategory;

                var res = await _categoryService.EditChildCategoryAsync(childCategory);

                if (res != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش زیر گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }
            }

            return Json(new { isValid = true, returnUrl = "current" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveChildCategory(int childCategoryId)
        {
            if (!await _categoryService.IsChildCategoryExistAsync(childCategoryId)) return NotFound();

            await _categoryService.RemoveChildCategoryAsync(childCategoryId);

            return RedirectToAction("Index");
        }
    }
}
