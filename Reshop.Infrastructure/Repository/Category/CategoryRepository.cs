using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Interfaces.Category;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<CategoriesDropdownViewModel> GetCategoriesDropdown() =>
            _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoriesDropdownViewModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryTitle = c.CategoryTitle,
                    CategoryImages = c.CategoryGalleries
                    .Select(g => new Tuple<string, string>(g.ImageName, g.ImageUrl))
                    .ToList(),
                    ChildCategories = c.ChildCategories
                    .Where(ch => ch.IsActive && ch.Products.Any())
                    .Select(ch => new ChildCategoriesOfCategoryForDropDown()
                    {
                        ChildCategoryId = ch.ChildCategoryId,
                        ChildCategoryName = ch.ChildCategoryTitle,
                        TopBrands = ch.BrandToChildCategories
                            .Select(b => b.Brand)
                            .OrderByDescending(p => p.OfficialBrandProducts
                                .SelectMany(b => b.Products).Count(b => b.IsActive && b.ShopperProducts.Any(i => i.IsActive && i.ShopperProductColors.Any(co => co.IsActive))))
                            .Take(5)
                            .Select(p => new Tuple<int, string, int>(
                                p.BrandId,
                                p.BrandName,
                                p.OfficialBrandProducts
                                    .SelectMany(b => b.Products).Count(b => b.IsActive && b.ShopperProducts.Any(i => i.IsActive && i.ShopperProductColors.Any(co => co.IsActive)))))
                    })
                });


        public async Task<Domain.Entities.Category.Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<ChildCategory> GetChildCategoryByIdAsync(int childCategoryId)
        {
            return await _context.ChildCategories.SingleOrDefaultAsync(c => c.ChildCategoryId == childCategoryId);
        }

        public async Task<int> GetChildCategoryIdOfProductAsync(int productId) =>
            await _context.Products.Where(c => c.ProductId == productId)
                .Select(c => c.ChildCategoryId).SingleOrDefaultAsync();

        public async Task<Tuple<int, string>> GetProductChildCategoryAsync(int productId) =>
            await _context.Products.Where(c => c.ProductId == productId)
               .Select(c => c.ChildCategory)
                .Select(c => new Tuple<int, string>(c.ChildCategoryId, c.ChildCategoryTitle))
               .SingleOrDefaultAsync();

        public async Task<Tuple<int, string>> GetProductCategoryAsync(int productId) =>
            await _context.Products.Where(c => c.ProductId == productId)
                .Select(c => c.ChildCategory)
                .Select(c => c.Category)
                .Select(c => new Tuple<int, string>(c.CategoryId, c.CategoryTitle))
                .SingleOrDefaultAsync();

        public IEnumerable<ChildCategory> GetChildCategoriesOfCategory(int categoryId) =>
            _context.Categories.Where(c => c.CategoryId == categoryId)
                .SelectMany(c => c.ChildCategories);


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

        public async Task AddCategoryGalleryAsync(CategoryGallery categoryGallery) =>
            await _context.CategoryGalleries.AddAsync(categoryGallery);

        public void RemoveCategoryGallery(CategoryGallery categoryGallery) =>
            _context.CategoryGalleries.Remove(categoryGallery);

        public void UpdateCategoryGallery(CategoryGallery categoryGallery) =>
            _context.CategoryGalleries.Update(categoryGallery);

        public async Task<CategoryGallery> GetCategoryGalleryAsync(int categoryId, string imageName) =>
            await _context.CategoryGalleries.Where(c => c.CategoryId == categoryId &&
                                                  c.ImageName == imageName).SingleOrDefaultAsync();

        public IEnumerable<Tuple<string, string>> GetCategoryGalleryImagesNameAndUrl(int categoryId) =>
            _context.CategoryGalleries.Where(c => c.CategoryId == categoryId)
                .OrderBy(c => c.OrderBy)
                .Select(c => new Tuple<string, string>(c.ImageName, c.ImageUrl));

        public void UpdateCategory(Domain.Entities.Category.Category category)
        {
            _context.Categories.Update(category);
        }

        public void UpdateChildCategory(ChildCategory childCategory)
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}