using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IProductRepository
    {
        #region product

        Task AddProductAsync(Entities.Product.Product product);
        void UpdateProduct(Entities.Product.Product product);
        Task<Entities.Product.Product> GetProductByIdAsync(int productId);
        Task<string> GetProductTypeAsync(int productId);

        #endregion

        #region get with pagination

        Task<IEnumerable<ProductViewModel>> GetProductsWithPaginationAsync(string type, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        IEnumerable<ProductDataForAdmin> GetProductsWithPaginationForAdmin(string type, int skip, int take, string filter);
        IEnumerable<ProductViewModel> GetProductsOfCategoryWithPagination(int categoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        IEnumerable<ProductViewModel> GetProductsOfChildCategoryWithPagination(int childCategoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        IEnumerable<ProductViewModel> GetProductsOfShopperWithPagination(string shopperId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        IEnumerable<ProductViewModel> GetProductsOfBrandWithPagination(int brandId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> officialBrandProducts = null);

        Task<decimal> GetMaxPriceOfProductsAsync(string type, string filter = null, List<int> brands = null);
        Task<decimal> GetMaxPriceOfCategoryProductsAsync(int categoryId, string filter = null, List<int> brands = null);
        Task<decimal> GetMaxPriceOfChildCategoryProductsAsync(int childCategoryId, string filter = null, List<int> brands = null);
        Task<decimal> GetMaxPriceOfShopperProductsAsync(string shopperId, string filter = null, List<int> brands = null);
        Task<decimal> GetMaxPriceOfBrandProductsAsync(int brandId, string filter = null, List<int> officialBrandProducts = null);

        Task<decimal> GetMinPriceOfProductsAsync(string type, string filter = null, List<int> brands = null);
        Task<decimal> GetMinPriceOfCategoryProductsAsync(int categoryId, string filter = null, List<int> brands = null);
        Task<decimal> GetMinPriceOfChildCategoryProductsAsync(int childCategoryId, string filter = null, List<int> brands = null);
        Task<decimal> GetMinPriceOfShopperProductsAsync(string shopperId, string filter = null, List<int> brands = null);
        Task<decimal> GetMinPriceOfBrandProductsAsync(int brandId, string filter = null, List<int> officialBrandProducts = null);

        #endregion

        #region product gallery

        IEnumerable<ProductGallery> GetProductImages(int productId);
        Task<string> GetProductFirstPictureName(int productId);
        Task<ProductGallery> GetProductGalleryAsync(int productId, string imageName);
        Task<int> GetProductGalleriesCountByProductIdAsync(int productId);
        Task AddProductGalley(ProductGallery productGallery);
        void RemoveProductGallery(ProductGallery productGallery);

        #endregion

        #region product items

        // ------------------------------ add ------------------------------
        Task AddMobileDetailAsync(MobileDetail mobileDetail);
        Task AddLaptopDetailAsync(LaptopDetail laptopDetail);
        Task AddPowerBankDetailAsync(PowerBankDetail powerBank);
        Task AddMobileCoverDetailAsync(MobileCoverDetail mobileCoverDetail);
        Task AddTabletDetailAsync(TabletDetail tabletDetail);
        Task AddFlashMemoryDetailAsync(FlashMemoryDetail flashMemoryDetail);
        Task AddSpeakerDetailAsync(SpeakerDetail speakerDetail);
        Task AddWristWatchDetailAsync(WristWatchDetail wristWatchDetail);
        Task AddSmartWatchAsync(SmartWatchDetail smartWatchDetail);
        Task AddMemoryCardDetailAsync(MemoryCardDetail memoryCardDetail);
        Task AddAUXDetailAsync(AUXDetail auxDetail);

        // ------------------------------ edit ------------------------------
        void UpdateMobileDetail(MobileDetail mobileDetail);
        void UpdateLaptopDetail(LaptopDetail laptopDetail);
        void UpdatePowerBankDetail(PowerBankDetail powerBank);
        void UpdateMobileCoverDetail(MobileCoverDetail mobileCoverDetail);
        void UpdateTabletDetail(TabletDetail tabletDetail);
        void UpdateFlashMemoryDetail(FlashMemoryDetail flashMemoryDetail);
        void UpdateSpeakerDetail(SpeakerDetail speakerDetail);
        void UpdateWristWatchDetail(WristWatchDetail wristWatchDetail);
        void UpdateSmartWatch(SmartWatchDetail smartWatchDetail);
        void UpdateMemoryCardDetail(MemoryCardDetail memoryCardDetail);
        void UpdateAUXDetail(AUXDetail auxDetail);

        // ------------------------------ get ------------------------------
        Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId);
        Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId);
        Task<PowerBankDetail> GetPowerBankDetailByIdAsync(int powerBankId);
        Task<MobileCoverDetail> GetMobileCoverDetailByIdAsync(int mobileCoverId);
        Task<TabletDetail> GetTabletDetailByIdAsync(int tabletDetailId);
        Task<FlashMemoryDetail> GetFlashMemoryDetailByIdAsync(int flashMemoryId);
        Task<SpeakerDetail> GetSpeakerDetailByIdAsync(int speakerDetailId);
        Task<SmartWatchDetail> GetSmartWatchDetailByIdAsync(int smartWatchDetailId);
        Task<WristWatchDetail> GetWristWatchDetailByIdAsync(int wristWatchDetailId);
        Task<MemoryCardDetail> GetMemoryCardDetailByIdAsync(int memoryCardDetailId);
        Task<AUXDetail> GetAUXByIdAsync(int auxId);

        // ------------------------------ get for edit ------------------------------
        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId);
        Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataForEditAsync(int productId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId);
        Task<AddOrEditAUXViewModel> GetTypeAUXProductDataForEditAsync(int productId);

        #endregion

        #region validation

        Task<bool> IsProductExistAsync(int productId);
        Task<bool> IsProductExistByShortKeyAsync(string shortKey);

        #endregion

        #region product detail

        Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId);
        Task<ProductDataForDetailViewModel> GetProductDataForDetailAsync(string shopperProductColorId);
        Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId);
        Task<ProductDataForCompareViewModel> GetProductDataForCompareAsync(int productId);

        #endregion

        Task<Tuple<string, string>> GetProductRedirectionByShortKeyAsync(string key);

        Task<Tuple<string, string>> GetBestSellerOfProductAsync(int productId);


        IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId, int colorId);
        Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(string shopperProductColorId);

        Task<int> GetProductIdOfShopperProductColorIdAsync(string shopperProductColorId);

        #region count

        Task<int> GetProductsCountWithTypeAsync(string type = "all");
        Task<int> GetUserFavoriteProductsCountWithTypeAsync(string userId);
        Task<int> GetProductsCountAsync(string type = "all", string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        Task<int> GetCategoryProductsCountAsync(int categoryId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        Task<int> GetChildCategoryProductsCountAsync(int childCategoryId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        Task<int> GetShopperProductsCountAsync(string shopperId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null);
        Task<int> GetBrandProductsCountAsync(int brandId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> officialBrandProducts = null);

        #endregion

        #region shopper product

        Task<ShopperProduct> GetShopperProductAsync(string shopperId, int productId);
        Task<ShopperProduct> GetShopperProductAsync(string shopperProductId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductId, int colorId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductColorId);

        #endregion


        #region favorite product

        IEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string sortBy, int skip = 0, int take = 24);
        Task AddToFavoriteProductAsync(FavoriteProduct favoriteProduct);
        void RemoveFavoriteProduct(FavoriteProduct favoriteProduct);
        void UpdateFavoriteProduct(FavoriteProduct favoriteProduct);
        Task<bool> IsFavoriteProductExistAsync(string favoriteProductId);
        Task<bool> IsFavoriteProductExistAsync(string userId, string shopperProductColorId);
        Task<FavoriteProduct> GetFavoriteProductAsync(string favoriteProductId);
        Task<FavoriteProduct> GetFavoriteProductAsync(string userId, string shopperProductColorId);

        #endregion

        #region Chart

        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductsDataChart();
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId);

        #endregion



        Task SaveChangesAsync();
    }
}