using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Interfaces.Category;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;

namespace Reshop.Application.Services.Product
{
    public class BrandService : IBrandService
    {
        #region constructor

        private readonly IBrandRepository _brandRepository;
        private readonly IShopperRepository _shopperRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BrandService(IBrandRepository brandRepository, IShopperRepository shopperRepository, ICategoryRepository categoryRepository)
        {
            _brandRepository = brandRepository;
            _shopperRepository = shopperRepository;
            _categoryRepository = categoryRepository;
        }

        #endregion

        public async Task<Brand> GetBrandByIdAsync(int brandId) =>
          await _brandRepository.GetBrandByIdAsync(brandId);

        public IEnumerable<Tuple<int, string>> GetBrandsForShow() =>
            _brandRepository.GetBrandsForShow();

        public async Task<ResultTypes> AddBrandAsync(Brand brand)
        {
            try
            {
                await _brandRepository.AddBrandAsync(brand);
                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditBrandAsync(Brand brand)
        {
            try
            {
                _brandRepository.UpdateBrand(brand);
                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> UnAvailableBrand(int brandId)
        {
            try
            {
                var brand = await _brandRepository.GetBrandByIdAsync(brandId);

                if (brand == null)
                    return ResultTypes.Failed;

                // UnAvailable Brand 
                brand.IsActive = false;

                var officialProducts = _brandRepository.GetRealOfficialProductsOfBrand(brandId);

                foreach (var officialBrandProduct in officialProducts)
                {
                    // UnAvailable OfficialBrandProduct
                    officialBrandProduct.IsActive = false;

                    var products = _brandRepository.GetRealProductsOfOfficialProduct(officialBrandProduct.OfficialBrandProductId);

                    // UnAvailable Products
                    foreach (var product in products)
                    {
                        product.IsActive = false;
                    }
                }

                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AvailableBrand(AvailableBrandViewModel model)
        {
            try
            {
                var brand = await _brandRepository.GetBrandByIdAsync(model.BrandId);

                if (brand == null)
                    return ResultTypes.Failed;

                // Available Brand 
                brand.IsActive = true;

                if (model.AvailableOfficialBrandProducts)
                {
                    var officialProducts = _brandRepository.GetRealOfficialProductsOfBrand(brand.BrandId);

                    foreach (var officialBrandProduct in officialProducts)
                    {
                        // Available OfficialBrandProduct
                        officialBrandProduct.IsActive = true;

                        if (model.AvailableProducts)
                        {
                            var products = _brandRepository.GetRealProductsOfOfficialProduct(officialBrandProduct.OfficialBrandProductId);

                            // Available Products
                            foreach (var product in products)
                            {
                                product.IsActive = true;
                            }
                        }
                    }
                }

                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<OfficialBrandProduct> GetOfficialBrandProductByIdAsync(int officialBrandProductId) =>
            await _brandRepository.GetOfficialBrandProductByIdAsync(officialBrandProductId);

        public async Task<ResultTypes> AddOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct)
        {
            try
            {
                await _brandRepository.AddOfficialBrandProductAsync(officialBrandProduct);
                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct)
        {
            try
            {
                _brandRepository.UpdateOfficialBrandProduct(officialBrandProduct);
                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>> GetBrandsForShowAsync(int pageId = 1, int take = 30, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int brandsCount = await _brandRepository.GetBrandsCountAsync();

            var brands = _brandRepository.GetBrandsForShow(skip, take, filter);


            int totalPages = (int)Math.Ceiling(1.0 * brandsCount / take);

            return new Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>(brands, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>> GetOfficialBrandProductsForShowAsync(int pageId = 1, int take = 30, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int officialBrandProductsCount = await _brandRepository.GetOfficialBrandProductsCountAsync();

            var officialBrandProducts = _brandRepository.GetOfficialBrandProductsForShow(skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * officialBrandProductsCount / take);

            return new Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>(officialBrandProducts, pageId, totalPages);
        }

        public IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId) =>
            _brandRepository.GetBrandOfficialProducts(brandId);

        public IEnumerable<ChildCategory> GetChildCategoriesOfBrand(int brandId) =>
            _brandRepository.GetChildCategoriesOfBrand(brandId);

        public IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId) =>
            _brandRepository.GetProductsOfOfficialProduct(officialProductId);

        public async Task<bool> IsBrandExistAsync(int brandId) =>
            await _brandRepository.IsBrandExistAsync(brandId);

        public async Task<bool> IsOfficialBrandProductExistAsync(int officialBrandProductId) =>
            await _brandRepository.IsOfficialBrandProductExistAsync(officialBrandProductId);

        public async Task<ResultTypes> AvailableOfficialBrandProductAsync(int officialBrandProductId, bool availableProducts)
        {
            try
            {
                var officialBrandProduct = await _brandRepository.GetOfficialBrandProductByIdAsync(officialBrandProductId);

                if (officialBrandProduct == null)
                    return ResultTypes.Failed;

                // Available Brand 
                officialBrandProduct.IsActive = true;

                if (availableProducts)
                {
                    var products = _brandRepository.GetRealProductsOfOfficialProduct(officialBrandProduct.OfficialBrandProductId);

                    // Available Products
                    foreach (var product in products)
                    {
                        product.IsActive = true;
                    }
                }

                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> UnAvailableOfficialBrandProductAsync(int officialBrandProductId)
        {
            try
            {
                var officialBrandProduct = await _brandRepository.GetOfficialBrandProductByIdAsync(officialBrandProductId);

                if (officialBrandProduct == null)
                    return ResultTypes.Failed;

                // un Available Brand 
                officialBrandProduct.IsActive = false;


                var products = _brandRepository.GetRealProductsOfOfficialProduct(officialBrandProduct.OfficialBrandProductId);

                // un Available Products
                foreach (var product in products)
                {
                    product.IsActive = false;
                }


                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId) =>
            _brandRepository.GetBrandsOfStoreTitle(storeTitleId);

        public async Task<ResultTypes> AddBrandToChildCategoriesByBrandAsync(int brandId, List<int> childCategoriesId)
        {
            try
            {
                foreach (int item in childCategoriesId)
                {
                    var brandToChildCategory = new BrandToChildCategory()
                    {
                        ChildCategoryId = item,
                        BrandId = brandId
                    };
                    await _brandRepository.AddBrandToChildCategoryAsync(brandToChildCategory);
                }


                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddBrandToChildCategoriesByChildCategoryAsync(int childCategoryId, List<int> brandId)
        {
            try
            {
                foreach (int item in brandId)
                {
                    var brandToChildCategory = new BrandToChildCategory()
                    {
                        ChildCategoryId = childCategoryId,
                        BrandId = item
                    };
                    await _brandRepository.AddBrandToChildCategoryAsync(brandToChildCategory);
                }


                await _brandRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task RemoveBrandToChildCategoriesByBrandAsync(int brandId)
        {
            var brandToChildCategories = _brandRepository.GetBrandToChildCategoriesByBrand(brandId);

            if (brandToChildCategories != null)
            {
                foreach (var brandToChildCategory in brandToChildCategories)
                {
                    _brandRepository.RemoveBrandToChildCategory(brandToChildCategory);
                }

                await _brandRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveBrandToChildCategoriesByChildCategoryAsync(int childCategoryId)
        {
            var brandToChildCategories = _brandRepository.GetBrandToChildCategoriesByChildCategory(childCategoryId);

            if (brandToChildCategories != null)
            {
                foreach (var brandToChildCategory in brandToChildCategories)
                {
                    _brandRepository.RemoveBrandToChildCategory(brandToChildCategory);
                }

                await _brandRepository.SaveChangesAsync();
            }
        }

        public async Task<AddOrEditBrandViewModel> GetBrandDataByBrandIdAsync(int brandId)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId);
            var selectedChildCategories = _brandRepository.GetChildCategoriesIdOfBrand(brandId);
            var storeTitles = _shopperRepository.GetStoreTitles();
            var childCategories = _categoryRepository.GetChildCategories();

            return new AddOrEditBrandViewModel()
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                LatinBrandName = brand.LatinBrandName,
                SelectedStoreTitleId = brand.StoreTitleId,
                StoreTitles = storeTitles,
                ChildCategories = childCategories,
                SelectedChildCategories = selectedChildCategories
            };
        }
    }
}