using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;

namespace Reshop.Domain.Interfaces.Category
{
    public interface ICategoryRepository
    {
        // get all
        IEnumerable<Entities.Category.Category> GetCategories();
        IEnumerable<ChildCategory> GetChildCategories();

        IAsyncEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown();

        // get by id
        Task<Entities.Category.Category> GetCategoryByIdAsync(int categoryId);
        Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId);

        IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId);
        IEnumerable<Entities.Category.Category> GetCategoriesOfChildCategory(int childCategoryId);
        IEnumerable<int> GetChildCategoriesIdOfCategory(int categoryId);
        IEnumerable<int> GetCategoriesIdOfChildCategory(int childCategoryId);
        IEnumerable<ChildCategoryToCategory> GetChildCategoryToCategoryByCategoryId(int categoryId);
        IEnumerable<ChildCategoryToCategory> GetChildCategoryToCategoryByChildCategoryId(int childCategoryId);
        IEnumerable<ChildCategory> GetProductChildCategories(int productId);

        // validations 
        Task<bool> IsCategoryExistAsync(int categoryId);
        Task<bool> IsChildCategoryExistAsync(int childCategoryId);

        // insert
        Task AddCategoryAsync(Entities.Category.Category category);
        Task AddChildCategoryAsync(ChildCategory childCategory);
        Task AddChildCategoryToCategoryAsync(ChildCategoryToCategory childCategoryToCategory);

        // update
        void EditCategory(Entities.Category.Category category);
        void EditChildCategory(ChildCategory childCategory);

        // remove
        void RemoveCategory(Entities.Category.Category category);
        void RemoveChildCategory(ChildCategory childCategory);
        void RemoveChildCategoryToCategory(ChildCategoryToCategory childCategoryToCategory);


        Task SaveChangesAsync();
    }
}