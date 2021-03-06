using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Category
{
    public interface ICategoryRepository
    {
        // get all
        IEnumerable<Entities.Category.Category> GetCategories();
        IEnumerable<ChildCategory> GetChildCategories();

        IEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown();
        IEnumerable<CategoriesMobileMenuViewModel> GetCategoriesMobileMenu();

        // get by id
        Task<Entities.Category.Category> GetCategoryByIdAsync(int categoryId);
        Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId);
        Task<int> GetChildCategoryIdOfProductAsync(int productId);
        Task<Tuple<int, string>> GetProductChildCategoryAsync(int productId);
        Task<Tuple<int, string>> GetProductCategoryAsync(int productId);
        IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId);

        // validations 
        Task<bool> IsCategoryExistAsync(int categoryId);
        Task<bool> IsChildCategoryExistAsync(int childCategoryId);

        // insert
        Task AddCategoryAsync(Entities.Category.Category category);
        Task AddChildCategoryAsync(ChildCategory childCategory);
        Task AddCategoryGalleryAsync(CategoryGallery categoryGallery);
        void RemoveCategoryGallery(CategoryGallery categoryGallery);
        void UpdateCategoryGallery(CategoryGallery categoryGallery);
        Task<CategoryGallery> GetCategoryGalleryAsync(int categoryId, string imageName);
        IEnumerable<Tuple<string, string>> GetCategoryGalleryImagesNameAndUrl(int categoryId);

        // update
        void UpdateCategory(Entities.Category.Category category);
        void UpdateChildCategory(ChildCategory childCategory);

        Task SaveChangesAsync();
    }
}