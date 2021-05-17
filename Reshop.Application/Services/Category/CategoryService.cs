using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Interfaces.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Services.Category
{
    public class CategoryService : ICategoryService
    {
        #region constructor

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion

        public IEnumerable<Domain.Entities.Category.Category> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }

        public IEnumerable<ChildCategory> GetChildCategories()
        {
            return _categoryRepository.GetChildCategories();
        }

        public IAsyncEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown()
            =>
                _categoryRepository.GetCategoriesDropdown();

        public async Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId)
        {
            return await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);
        }

        public async Task UpdateChildCategoryAsync(ChildCategory childCategory)
        {
            _categoryRepository.EditChildCategory(childCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryExistAsync(int categoryId)
        {
            return await _categoryRepository.IsCategoryExistAsync(categoryId);
        }

        public async Task<bool> IsChildCategoryExistAsync(int childCategoryId)
        {
            return await _categoryRepository.IsChildCategoryExistAsync(childCategoryId);
        }

        public async Task RemoveChildCategoryToCategoryByCategoryIdAsync(int categoryId)
        {
            var childCategoryToCategories = _categoryRepository.GetChildCategoryToCategoryByCategoryId(categoryId);

            if (childCategoryToCategories != null)
            {
                foreach (var childCategoryToCategory in childCategoryToCategories)
                {
                    _categoryRepository.RemoveChildCategoryToCategory(childCategoryToCategory);
                }

                await _categoryRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveChildCategoryToCategoryByChildCategoryIdAsync(int childCategoryId)
        {
            var childCategoryToCategories = _categoryRepository.GetChildCategoryToCategoryByChildCategoryId(childCategoryId);

            if (childCategoryToCategories != null)
            {
                foreach (var childCategoryToCategory in childCategoryToCategories)
                {
                    _categoryRepository.RemoveChildCategoryToCategory(childCategoryToCategory);
                }

                await _categoryRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category != null)
            {
                var childCategoryToCategories = _categoryRepository.GetChildCategoryToCategoryByCategoryId(categoryId);

                if (childCategoryToCategories != null)
                {
                    foreach (var childCategoryToCategory in childCategoryToCategories)
                    {
                        _categoryRepository.RemoveChildCategoryToCategory(childCategoryToCategory);
                    }

                    await _categoryRepository.SaveChangesAsync();
                }
            }

            _categoryRepository.RemoveCategory(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task RemoveChildCategoryAsync(int childCategoryId)
        {
            var childCategory = await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);

            if (childCategory != null)
            {
                var childCategoryToCategories = _categoryRepository.GetChildCategoryToCategoryByChildCategoryId(childCategoryId);

                if (childCategoryToCategories != null)
                {
                    foreach (var childCategoryToCategory in childCategoryToCategories)
                    {
                        _categoryRepository.RemoveChildCategoryToCategory(childCategoryToCategory);
                    }

                    await _categoryRepository.SaveChangesAsync();
                }
            }

            _categoryRepository.RemoveChildCategory(childCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId)
        {
            return _categoryRepository.GetChildCategoriesOfCategory(categoryId);
        }

        public IEnumerable<Domain.Entities.Category.Category> GetCategoriesOfChildCategory(int childCategoryId)
        {
            return _categoryRepository.GetCategoriesOfChildCategory(childCategoryId);
        }

        public async Task<AddOrEditChildCategoryViewModel> GetChildCategoryDataAsync(int childCategoryId)
        {
            var childCategory = await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);
            var categories = _categoryRepository.GetCategories();
            var categoryOfChildCategory = _categoryRepository.GetCategoriesIdOfChildCategory(childCategoryId);

            return new AddOrEditChildCategoryViewModel()
            {
                ChildCategoryId = childCategory.ChildCategoryId,
                ChildCategoryTitle = childCategory.ChildCategoryTitle,
                Categories = categories,
                SelectedCategories = categoryOfChildCategory
            };
        }

        public async Task AddCategoryAsync(Domain.Entities.Category.Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task AddChildCategoryAsync(ChildCategory childCategory)
        {
            await _categoryRepository.AddChildCategoryAsync(childCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task AddChildCategoryToCategoryAsync(ChildCategoryToCategory childCategoryToCategory)
        {
            await _categoryRepository.AddChildCategoryToCategoryAsync(childCategoryToCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<AddOrEditCategoryViewModel> GetCategoryDataAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            var childCategories = _categoryRepository.GetChildCategories();
            var childCategoriesOfCategory = _categoryRepository.GetChildCategoriesIdOfCategory(categoryId);

            return new AddOrEditCategoryViewModel()
            {
                CategoryId = category.CategoryId,
                CategoryTitle = category.CategoryTitle,
                ChildCategories = childCategories,
                SelectedChildCategories = childCategoriesOfCategory
            };
        }

        public async Task UpdateCategoryAsync(Domain.Entities.Category.Category category)
        {
            _categoryRepository.EditCategory(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
