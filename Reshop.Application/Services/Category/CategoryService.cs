using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Interfaces.Category;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown() =>
            _categoryRepository.GetCategoriesDropdown();

        public IEnumerable<CategoriesMobileMenuViewModel> GetCategoriesMobileMenu() =>
            _categoryRepository.GetCategoriesMobileMenu();

        public async Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId)
        {
            return await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);
        }

        public async Task<ResultTypes> EditChildCategoryAsync(ChildCategory childCategory)
        {
            try
            {
                _categoryRepository.UpdateChildCategory(childCategory);
                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsCategoryExistAsync(int categoryId)
        {
            return await _categoryRepository.IsCategoryExistAsync(categoryId);
        }

        public async Task<bool> IsChildCategoryExistAsync(int childCategoryId)
        {
            return await _categoryRepository.IsChildCategoryExistAsync(childCategoryId);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category != null)
            {
                var childCategories = _categoryRepository.GetChildCategoriesOfCategory(categoryId);

                if (childCategories != null)
                {
                    foreach (var childCategory in childCategories)
                    {
                        childCategory.IsActive = false;


                        _categoryRepository.UpdateChildCategory(childCategory);
                    }

                    await _categoryRepository.SaveChangesAsync();
                }

                category.IsActive = false;

                _categoryRepository.UpdateCategory(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveChildCategoryAsync(int childCategoryId)
        {
            var childCategory = await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);

            if (childCategory != null)
            {
                childCategory.IsActive = false;

                _categoryRepository.UpdateChildCategory(childCategory);
                await _categoryRepository.SaveChangesAsync();
            }
        }

        public async Task<AddOrEditChildCategoryViewModel> GetChildCategoryDataAsync(int childCategoryId)
        {
            var childCategory = await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);
            var categories = _categoryRepository.GetCategories();

            return new AddOrEditChildCategoryViewModel()
            {
                ChildCategoryId = childCategory.ChildCategoryId,
                ChildCategoryTitle = childCategory.ChildCategoryTitle,
                Categories = categories,
                IsActive = childCategory.IsActive,
                SelectedCategory = childCategory.CategoryId,
            };
        }

        public async Task<AddOrEditCategoryViewModel> GetCategoryDataAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            var imagesName = _categoryRepository.GetCategoryGalleryImagesNameAndUrl(category.CategoryId).ToList();

            return new AddOrEditCategoryViewModel()
            {
                CategoryId = category.CategoryId,
                CategoryTitle = category.CategoryTitle,
                IsActive = category.IsActive,
                SelectedImage1IMG = imagesName.Skip(0).First().Item1,
                SelectedImage2IMG = imagesName.Skip(1).First().Item1,
                SelectedImage3IMG = imagesName.Skip(2).First().Item1,
                SelectedImage4IMG = imagesName.Skip(3).First().Item1,
                SelectedImage5IMG = imagesName.Skip(4).First().Item1,
                SelectedImage6IMG = imagesName.Skip(5).First().Item1,
                SelectedImage1URL = imagesName.Skip(0).First().Item2,
                SelectedImage2URL = imagesName.Skip(1).First().Item2,
                SelectedImage3URL = imagesName.Skip(2).First().Item2,
                SelectedImage4URL = imagesName.Skip(3).First().Item2,
                SelectedImage5URL = imagesName.Skip(4).First().Item2,
                SelectedImage6URL = imagesName.Skip(5).First().Item2,
            };
        }

        public async Task<int> GetChildCategoryIdOfProductAsync(int productId) =>
            await _categoryRepository.GetChildCategoryIdOfProductAsync(productId);

        public async Task<ResultTypes> AddCategoryAsync(Domain.Entities.Category.Category category)
        {
            try
            {
                await _categoryRepository.AddCategoryAsync(category);
                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddChildCategoryAsync(ChildCategory childCategory)
        {
            try
            {
                await _categoryRepository.AddChildCategoryAsync(childCategory);
                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddCategoryGalleryAsync(CategoryGallery categoryGallery)
        {
            try
            {
                await _categoryRepository.AddCategoryGalleryAsync(categoryGallery);

                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<CategoryGallery> GetCategoryGalleryAsync(int categoryId, string imageName) =>
            await _categoryRepository.GetCategoryGalleryAsync(categoryId, imageName);

        public async Task<ResultTypes> DeleteCategoryGalleryAsync(CategoryGallery categoryGallery)
        {
            try
            {
                _categoryRepository.RemoveCategoryGallery(categoryGallery);

                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditCategoryGalleryAsync(CategoryGallery categoryGallery)
        {
            try
            {
                _categoryRepository.UpdateCategoryGallery(categoryGallery);

                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditCategoryAsync(Domain.Entities.Category.Category category)
        {
            try
            {
                _categoryRepository.UpdateCategory(category);

                if (!category.IsActive)
                {
                    var childCategories = _categoryRepository.GetChildCategoriesOfCategory(category.CategoryId);

                    if (childCategories != null)
                    {
                        foreach (var childCategory in childCategories)
                        {
                            childCategory.IsActive = false;


                            _categoryRepository.UpdateChildCategory(childCategory);
                        }

                        await _categoryRepository.SaveChangesAsync();
                    }
                }

                await _categoryRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }
    }
}
