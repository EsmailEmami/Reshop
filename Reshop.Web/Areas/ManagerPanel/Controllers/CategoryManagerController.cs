using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class CategoryManagerController : Controller
    {
        #region constructor

        private readonly ICategoryService _categoryService;

        public CategoryManagerController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_categoryService.GetCategories());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCategory(int categoryId = 0)
        {
            if (categoryId == 0)
            {
                var model = new AddOrEditCategoryViewModel()
                {
                    ChildCategories = _categoryService.GetChildCategories()
                };

                return View(model);
            }
            else
            {
                var category = await _categoryService.GetCategoryDataAsync(categoryId);

                if (category is null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


                return View(category);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryViewModel model)
        {
            model.ChildCategories = _categoryService.GetChildCategories();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });



            if (model.CategoryId == 0)
            {
                var category = new Category()
                {
                    CategoryTitle = model.CategoryTitle,
                };

                var res = await _categoryService.AddCategoryAsync(category);

                if (res == ResultTypes.Successful)
                {
                    if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                    {
                        await _categoryService.AddChildCategoryToCategoryAsync(category.CategoryId, model.SelectedChildCategories as List<int>);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "متاسفاه هنگام افزودن گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
                }
            }
            else
            {

                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId);

                if (category == null)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
                }


                // update category
                category.CategoryTitle = model.CategoryTitle;

                var res = await _categoryService.EditCategoryAsync(category);

                if (res != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفاه هنگام ویرایش گروه به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
                }


                // remove all child categories
                await _categoryService.RemoveChildCategoryToCategoryByCategoryIdAsync(category.CategoryId);


                // add selected child categories
                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    await _categoryService.AddChildCategoryToCategoryAsync(category.CategoryId, model.SelectedChildCategories as List<int>);
                }
            }

            return Json(new { isValid = true, returnUrl = "current" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            if (!await _categoryService.IsCategoryExistAsync(categoryId)) return NotFound();

            await _categoryService.RemoveCategoryAsync(categoryId);

            return RedirectToAction("Index");
        }


    }
}
