using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;

namespace Reshop.Application.Interfaces.Product
{
    public interface IBrandService
    {
        Task<Brand> GetBrandByIdAsync(int brandId);
        IEnumerable<Tuple<int, string>> GetBrandsForShow();
        Task<ResultTypes> AddBrandAsync(Brand brand);
        Task<ResultTypes> EditBrandAsync(Brand brand);
        Task<ResultTypes> UnAvailableBrand(int brandId);
        Task<ResultTypes> AvailableBrand(AvailableBrandViewModel model);
        Task<OfficialBrandProduct> GetOfficialBrandProductByIdAsync(int officialBrandProductId);
        Task<ResultTypes> AddOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct);
        Task<ResultTypes> EditOfficialBrandProductAsync(OfficialBrandProduct officialBrandProduct);
        Task<Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>> GetBrandsForShowAsync(int pageId = 1, int take = 30, string filter = "");
        Task<Tuple<IEnumerable<Tuple<int, string, bool>>, int, int>> GetOfficialBrandProductsForShowAsync(int pageId = 1, int take = 30, string filter = "");
        IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId);
        IEnumerable<ChildCategory> GetChildCategoriesOfBrand(int brandId);
        IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId);
        Task<bool> IsBrandExistAsync(int brandId);
        Task<bool> IsOfficialBrandProductExistAsync(int officialBrandProductId);
        Task<ResultTypes> AvailableOfficialBrandProductAsync(int officialBrandProductId, bool availableProducts);
        Task<ResultTypes> UnAvailableOfficialBrandProductAsync(int officialBrandProductId);
        IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId);


        #region brand to childcategory

        Task<ResultTypes> AddBrandToChildCategoriesByBrandAsync(int brandId, List<int> childCategoriesId);
        Task<ResultTypes> AddBrandToChildCategoriesByChildCategoryAsync(int childCategoryId, List<int> brandId);
        Task RemoveBrandToChildCategoriesByBrandAsync(int brandId);
        Task RemoveBrandToChildCategoriesByChildCategoryAsync(int childCategoryId);
        Task<AddOrEditBrandViewModel> GetBrandDataByBrandIdAsync(int brandId);

        #endregion
    }
}