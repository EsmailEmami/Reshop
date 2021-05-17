using Microsoft.AspNetCore.Authorization;
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
                if (!await _categoryService.IsCategoryExistAsync(categoryId))
                    return NotFound();

                return View(await _categoryService.GetCategoryDataAsync(categoryId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CategoryId == 0)
            {
                var category = new Category()
                {
                    CategoryTitle = model.CategoryTitle,
                };

                await _categoryService.AddCategoryAsync(category);

                if (model.SelectedChildCategories != null && model.SelectedChildCategories.Any())
                {
                    foreach (int childCategoryId in model.SelectedChildCategories)
                    {
                        var childCategoryToCategory = new ChildCategoryToCategory()
                        {
                            CategoryId = category.CategoryId,
                            ChildCategoryId = childCategoryId,
                        };

                        await _categoryService.AddChildCategoryToCategoryAsync(childCategoryToCategory);
                    }
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
                    foreach (int childCategoryId in model.SelectedChildCategories)
                    {
                        var childCategoryToCategory = new ChildCategoryToCategory()
                        {
                            CategoryId = category.CategoryId,
                            ChildCategoryId = childCategoryId,
                        };

                        await _categoryService.AddChildCategoryToCategoryAsync(childCategoryToCategory);
                    }
                }
            }

            return RedirectToAction("Index");
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
