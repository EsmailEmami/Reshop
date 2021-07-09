using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Reshop.Application.Convertors;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
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
                {
                    return NotFound();
                }


                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryViewModel model)
        {
            model.ChildCategories = _categoryService.GetChildCategories();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddOrEditCategory", model) });



            if (model.CategoryId ==0)
            {
                var category = new Category()
                {
                    CategoryTitle = model.CategoryTitle,
                };

                await _categoryService.AddCategoryAsync(category);

                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    await _categoryService.AddChildCategoryToCategoryAsync(category.CategoryId, model.SelectedChildCategories as List<int>);
                }
            }
            else
            {
           

  


                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId);

                if (category == null) return NotFound();

                // update category
                category.CategoryTitle = model.CategoryTitle;

                await _categoryService.UpdateCategoryAsync(category);

                // remove all child categories
                await _categoryService.RemoveChildCategoryToCategoryByCategoryIdAsync(category.CategoryId);


                // add selected child categories
                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    await _categoryService.AddChildCategoryToCategoryAsync(category.CategoryId, model.SelectedChildCategories as List<int>);
                }
            }

            return Json(new { isValid = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            if (!await _categoryService.IsCategoryExistAsync(categoryId)) return NotFound();

            await _categoryService.RemoveCategoryAsync(categoryId);

            return RedirectToAction("Index");
        }


    }
}
