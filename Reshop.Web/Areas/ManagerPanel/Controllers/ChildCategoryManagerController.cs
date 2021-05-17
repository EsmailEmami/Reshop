﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
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
                if (!await _categoryService.IsChildCategoryExistAsync(childCategoryId))
                    return NotFound();

                return View(await _categoryService.GetChildCategoryDataAsync(childCategoryId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditChildCategory(AddOrEditChildCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.ChildCategoryId == 0)
            {
                var childCategory = new ChildCategory()
                {
                    ChildCategoryTitle = model.ChildCategoryTitle,
                };

                await _categoryService.AddChildCategoryAsync(childCategory);

                if (model.SelectedCategories != null)
                {
                    foreach (int categoryId in model.SelectedCategories)
                    {
                        var childCategoryToCategory = new ChildCategoryToCategory()
                        {
                            CategoryId = categoryId,
                            ChildCategoryId = childCategory.ChildCategoryId,
                        };

                        await _categoryService.AddChildCategoryToCategoryAsync(childCategoryToCategory);
                    }
                }
            }
            else
            {
                var childCategory = await _categoryService.GetChildCategoryByIdAsync(model.ChildCategoryId);

                if (childCategory == null) return NotFound();

                // update category
                childCategory.ChildCategoryTitle = model.ChildCategoryTitle;

                await _categoryService.UpdateChildCategoryAsync(childCategory);

                // remove all child categories
                await _categoryService.RemoveChildCategoryToCategoryByChildCategoryIdAsync(childCategory.ChildCategoryId);


                // add selected child categories
                if (model.SelectedCategories != null && model.SelectedCategories.Any())
                {
                    foreach (int categoryId in model.SelectedCategories)
                    {
                        var childCategoryToCategory = new ChildCategoryToCategory()
                        {
                            CategoryId = categoryId,
                            ChildCategoryId = childCategory.ChildCategoryId,
                        };

                        await _categoryService.AddChildCategoryToCategoryAsync(childCategoryToCategory);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveChildCategory(int childCategoryId)
        {
            if (!await _categoryService.IsChildCategoryExistAsync(childCategoryId)) return NotFound();

            await _categoryService.RemoveChildCategoryAsync(childCategoryId);

            return RedirectToAction("Index");
        }
    }
}
