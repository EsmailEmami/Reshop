using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IBrandRepository
    {
        #region brand

        IEnumerable<string> GetBrandsOfCategory(int categoryId);
        IEnumerable<string> GetBrandsOfChildCategory(int childCategoryId);
        Task<Brand> GetBrandByIdAsync(int brandId);
        IEnumerable<Tuple<int, string>> GetBrandsForShow();
        IEnumerable<Tuple<int, string, bool>> GetBrandsForShow(int skip, int take, string filter);
        Task<int> GetBrandsCountAsync();
        Task AddBrandAsync(Brand brand);
        void UpdateBrand(Brand brand);
        Task<bool> IsBrandExistAsync(int brandId);

        IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId);

        #endregion

        #region official brand product

        IEnumerable<Tuple<int, string, bool>> GetOfficialBrandProductsForShow(int skip, int take, string filter = "");
        IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId);
        IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId);
        Task<int> GetOfficialBrandProductsCountAsync();
        Task<OfficialBrandProduct> GetOfficialBrandProductByIdAsync(int officialBrandProductId);
        IEnumerable<OfficialBrandProduct> GetRealOfficialProductsOfBrand(int brandId);
        IEnumerable<Entities.Product.Product> GetRealProductsOfOfficialProduct(int officialProductId);
        Task AddOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct);
        void UpdateOfficialBrandProduct(OfficialBrandProduct officialBrandProduct);
        Task<bool> IsOfficialBrandProductExistAsync(int officialBrandProductId);

        #endregion



        Task SaveChangesAsync();
    }
}