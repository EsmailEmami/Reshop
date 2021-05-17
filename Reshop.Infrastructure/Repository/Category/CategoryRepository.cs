using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Interfaces.Category;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Category;

namespace Reshop.Infrastructure.Repository.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public CategoryRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion


        public IEnumerable<Domain.Entities.Category.Category> GetCategories()
        {
            return _context.Categories;
        }

        public IEnumerable<ChildCategory> GetChildCategories()
        {
            return _context.ChildCategories;
        }

        public IAsyncEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown()
            =>
                _context.Categories.Select(c => new CategoriesDropdownViewModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryTitle = c.CategoryTitle,
                    ChildCategories = _context.ChildCategoryToCategories.Where(g => g.CategoryId == c.CategoryId)
                        .Select(b => b.ChildCategory).ToList(),
                }) as IAsyncEnumerable<CategoriesDropdownViewModel>;


        public async Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId)
        {
            return await _context.ChildCategories.SingleOrDefaultAsync(c => c.ChildCategoryId == childCategoryId);
        }

        public IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId)
        {
            return _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategory);
        }

        public IEnumerable<Domain.Entities.Category.Category> GetCategoriesOfChildCategory(int childCategoryId)
        {
            return _context.ChildCategoryToCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Category);
        }

        public IEnumerable<int> GetChildCategoriesIdOfCategory(int categoryId)
        {
            return _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategoryId);
        }

        public IEnumerable<int> GetCategoriesIdOfChildCategory(int childCategoryId)
        {
            return _context.ChildCategoryToCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.CategoryId);
        }

        public IEnumerable<ChildCategoryToCategory> GetChildCategoryToCategoryByCategoryId(int categoryId)
        {
            return _context.ChildCategoryToCategories.Where(c => c.CategoryId == categoryId);
        }

        public IEnumerable<ChildCategoryToCategory> GetChildCategoryToCategoryByChildCategoryId(int childCategoryId)
        {
            return _context.ChildCategoryToCategories.Where(c => c.ChildCategoryId == childCategoryId);
        }

        public async Task<bool> IsCategoryExistAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> IsChildCategoryExistAsync(int childCategoryId)
        {
            return await _context.ChildCategories.AnyAsync(c => c.ChildCategoryId == childCategoryId);
        }

        public async Task AddCategoryAsync(Domain.Entities.Category.Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task AddChildCategoryAsync(ChildCategory childCategory)
        {
            await _context.ChildCategories.AddAsync(childCategory);
        }

        public async Task AddChildCategoryToCategoryAsync(ChildCategoryToCategory childCategoryToCategory)
        {
            await _context.ChildCategoryToCategories.AddAsync(childCategoryToCategory);
        }

        public void EditCategory(Domain.Entities.Category.Category category)
        {
            _context.Categories.Update(category);
        }

        public void EditChildCategory(ChildCategory childCategory)
        {
            _context.ChildCategories.Update(childCategory);
        }

        public void RemoveCategory(Domain.Entities.Category.Category category)
        {
            _context.Categories.Remove(category);
        }

        public void RemoveChildCategory(ChildCategory childCategory)
        {
            _context.ChildCategories.Remove(childCategory);
        }

        public void RemoveChildCategoryToCategory(ChildCategoryToCategory childCategoryToCategory)
        {
            _context.ChildCategoryToCategories.Remove(childCategoryToCategory);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}