using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;

namespace Reshop.Application.Interfaces.Category
{
    public interface ICategoryService
    {
        // get all
        IEnumerable<Domain.Entities.Category.Category> GetCategories();
        IEnumerable<ChildCategory> GetChildCategories();

        IAsyncEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown();

        // get by id
        Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId);
        Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId);
        IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId);
        IEnumerable<Domain.Entities.Category.Category> GetCategoriesOfChildCategory(int childCategoryId);
        Task<AddOrEditCategoryViewModel> GetCategoryDataAsync(int categoryId);
        Task<AddOrEditChildCategoryViewModel> GetChildCategoryDataAsync(int childCategoryId);

        // insert
        Task AddCategoryAsync(Domain.Entities.Category.Category category);
        Task AddChildCategoryAsync(ChildCategory childCategory);
        Task AddChildCategoryToCategoryAsync(ChildCategoryToCategory childCategoryToCategory);

        // update 
        Task UpdateCategoryAsync(Domain.Entities.Category.Category category);
        Task UpdateChildCategoryAsync(ChildCategory childCategory);

        // validations 
        Task<bool> IsCategoryExistAsync(int categoryId);
        Task<bool> IsChildCategoryExistAsync(int childCategoryId);

        // remove
        Task RemoveChildCategoryToCategoryByCategoryIdAsync(int categoryId);
        Task RemoveChildCategoryToCategoryByChildCategoryIdAsync(int childCategoryId);
        Task RemoveCategoryAsync(int categoryId);
        Task RemoveChildCategoryAsync(int childCategoryId);
    }
}