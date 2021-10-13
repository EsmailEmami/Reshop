using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Interfaces.Product;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Product
{
    public class BrandRepository : IBrandRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public BrandRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public IEnumerable<Tuple<int, string>> GetBrandsOfCategory(int categoryId) =>
            _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategory)
                .SelectMany(c => c.BrandToChildCategories)
                .Select(c => c.Brand)
                .Select(c => new Tuple<int, string>(c.BrandId, c.BrandName));

        public IEnumerable<Tuple<int, string>> GetBrandsOfChildCategory(int childCategoryId) =>
            _context.BrandToChildCategories.Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Brand)
                .Select(c => new Tuple<int, string>(c.BrandId, c.BrandName));

        public async Task<Brand> GetBrandByIdAsync(int brandId) =>
        await _context.Brands.FindAsync(brandId);

        public IEnumerable<Tuple<int, string>> GetBrandsForShow() =>
            _context.Brands
                .Select(c => new Tuple<int, string>(c.BrandId, c.BrandName));

        public IEnumerable<Tuple<int, string, bool>> GetBrandsForShow(int skip, int take, string filter)
        {
            IQueryable<Brand> brands = _context.Brands;

            if (filter != null)
            {
                brands = brands.Where(c => c.BrandName.Contains(filter));
            }

            brands = brands.Skip(skip).Take(take);

            return brands.Select(c => new Tuple<int, string, bool>(c.BrandId, c.BrandName, c.IsActive));
        }

        public IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId) =>
            _context.StoreTitles.Where(c => c.StoreTitleId == storeTitleId)
                .SelectMany(c => c.Brands)
                .Select(c => new Tuple<int, string>(c.BrandId, c.BrandName));


        public IEnumerable<Tuple<int, string, bool>> GetOfficialBrandProductsForShow(int skip, int take, string filter = "")
        {
            IQueryable<OfficialBrandProduct> officialBrandProducts = _context.OfficialBrandProducts;

            if (filter != null)
            {
                officialBrandProducts = officialBrandProducts.Where(c => c.OfficialBrandProductName.Contains(filter));
            }

            officialBrandProducts = officialBrandProducts.Skip(skip).Take(take);

            return officialBrandProducts.Select(c => new Tuple<int, string, bool>(c.OfficialBrandProductId, $"{c.OfficialBrandProductName} ({c.Brand.BrandName})", c.IsActive));
        }

        public IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId) =>
            _context.Brands.Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts)
                .Select(c => new Tuple<int, string>(c.OfficialBrandProductId, c.OfficialBrandProductName));

        public IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId) =>
            _context.OfficialBrandProducts.Where(c => c.OfficialBrandProductId == officialProductId)
                .SelectMany(c => c.Products)
                .Select(c => new Tuple<int, string>(c.ProductId, c.ProductTitle));

        public async Task<int> GetBrandsCountAsync() =>
            await _context.Brands.CountAsync();

        public async Task<int> GetOfficialBrandProductsCountAsync() =>
            await _context.OfficialBrandProducts.CountAsync();

        public async Task AddBrandAsync(Brand brand) =>
            await _context.Brands.AddAsync(brand);

        public void UpdateBrand(Brand brand) =>
            _context.Brands.Update(brand);

        public async Task<OfficialBrandProduct> GetOfficialBrandProductByIdAsync(int officialBrandProductId) =>
            await _context.OfficialBrandProducts.FindAsync(officialBrandProductId);

        public IEnumerable<OfficialBrandProduct> GetRealOfficialProductsOfBrand(int brandId) =>
            _context.Brands.Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts);

        public IEnumerable<Domain.Entities.Product.Product> GetRealProductsOfOfficialProduct(int officialProductId) =>
            _context.OfficialBrandProducts
                .Where(c => c.OfficialBrandProductId == officialProductId)
                .SelectMany(c => c.Products);

        public async Task AddOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct) =>
            await _context.OfficialBrandProducts.AddAsync(officialBrandProduct);

        public void UpdateOfficialBrandProduct(OfficialBrandProduct officialBrandProduct) =>
            _context.OfficialBrandProducts.Update(officialBrandProduct);

        public async Task<bool> IsBrandExistAsync(int brandId) =>
            await _context.Brands.AnyAsync(c => c.BrandId == brandId);

        public async Task<bool> IsOfficialBrandProductExistAsync(int officialBrandProductId) =>
            await _context.OfficialBrandProducts.AnyAsync(c => c.OfficialBrandProductId == officialBrandProductId);

        public async Task AddBrandToChildCategoryAsync(BrandToChildCategory brandToChildCategory) =>
            await _context.BrandToChildCategories.AddAsync(brandToChildCategory);

        public void RemoveBrandToChildCategory(BrandToChildCategory brandToChildCategory) =>
            _context.BrandToChildCategories.Remove(brandToChildCategory);

        public IEnumerable<BrandToChildCategory> GetBrandToChildCategoriesByBrand(int brandId) =>
            _context.BrandToChildCategories.Where(c => c.BrandId == brandId);

        public IEnumerable<BrandToChildCategory> GetBrandToChildCategoriesByChildCategory(int childCategoryId) =>
            _context.BrandToChildCategories.Where(c => c.ChildCategoryId == childCategoryId);

        public IEnumerable<int> GetBrandsIdOfChildCategory(int childCategoryId) =>
            _context.BrandToChildCategories.Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.BrandId);

        public IEnumerable<int> GetChildCategoriesIdOfBrand(int brandId) =>
            _context.BrandToChildCategories.Where(c => c.BrandId == brandId)
                .Select(c => c.ChildCategoryId);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}