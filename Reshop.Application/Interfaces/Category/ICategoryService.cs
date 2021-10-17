using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;

namespace Reshop.Application.Interfaces.Category
{
    public interface ICategoryService
    {
        // get all
        IEnumerable<Domain.Entities.Category.Category> GetCategories();
        IEnumerable<ChildCategory> GetChildCategories();

        IEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown();

        // get by id
        Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId);
        Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId);
        IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId);
        IEnumerable<Domain.Entities.Category.Category> GetCategoriesOfChildCategory(int childCategoryId);
        Task<AddOrEditCategoryViewModel> GetCategoryDataAsync(int categoryId);
        Task<AddOrEditChildCategoryViewModel> GetChildCategoryDataAsync(int childCategoryId);

        // insert
        Task<ResultTypes> AddCategoryAsync(Domain.Entities.Category.Category category);
        Task<ResultTypes> AddChildCategoryAsync(ChildCategory childCategory);
        Task AddChildCategoryToCategoryAsync(int categoryId, List<int> childCategoriesId);
        Task AddCategoryToChildCategoryAsync(int childCategoryId, List<int> categoriesId);
        Task<ResultTypes> AddProductToChildCategoryByProductAsync(int productId, List<int> childCategoryIds);
        Task<ResultTypes> AddCategoryGalleryAsync(CategoryGallery categoryGallery);
        Task<CategoryGallery> GetCategoryGalleryAsync(int categoryId, string imageName);
        Task<ResultTypes> DeleteCategoryGalleryAsync(CategoryGallery categoryGallery);
        Task<ResultTypes> EditCategoryGalleryAsync(CategoryGallery categoryGallery);

        // update 
        Task<ResultTypes> EditCategoryAsync(Domain.Entities.Category.Category category);
        Task<ResultTypes> EditChildCategoryAsync(ChildCategory childCategory);

        // validations 
        Task<bool> IsCategoryExistAsync(int categoryId);
        Task<bool> IsChildCategoryExistAsync(int childCategoryId);

        // remove
        Task RemoveChildCategoryToCategoryByCategoryIdAsync(int categoryId);
        Task RemoveChildCategoryToCategoryByChildCategoryIdAsync(int childCategoryId);
        Task RemoveProductToChildCategoryByProductIdAsync(int productId);
        Task RemoveCategoryAsync(int categoryId);
        Task RemoveChildCategoryAsync(int childCategoryId);
    }
}