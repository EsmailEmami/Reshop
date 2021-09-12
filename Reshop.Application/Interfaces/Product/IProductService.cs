﻿using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;

namespace Reshop.Application.Interfaces.Product
{
    public interface IProductService
    {

        IEnumerable<ProductViewModel> GetProductsWithType(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int take = 18);

        // product , pageId , totalPages
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetProductsWithPaginationAsync(string type = "all", string sortBy = "news", int pageId = 1, int take = 18);
        Task<Tuple<IEnumerable<ProductDataForAdmin>, int, int>> GetProductsWithPaginationForAdminAsync(string type = "all", int pageId = 1, int take = 18, string filter = "");
        Task<CategoryOrChildCategoryProductsForShow> GetCategoryProductsWithPaginationAsync(int categoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null);
        Task<CategoryOrChildCategoryProductsForShow> GetChildCategoryProductsWithPaginationAsync(int childCategoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null);

        // this is for list of shopper products
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetShopperProductsWithPaginationAsync(string shopperId, string type = "all", string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null);

        Task<int> GetProductsCountWithTypeAsync(string type = "all");
        // get by id 
        Task<ProductDetailForShow> GetProductDetailForShopperAsync(int productId, string shopperId);
        Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId);
        Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId);
        Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId);
        Task<ShopperProduct> GetShopperProductAsync(int productId, string shopperId);
        Task<ShopperProduct> GetShopperProductAsync(string shopperProductId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(int productId, string shopperId, int colorId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductColorId);
        Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId);
        Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId);
        Task<PowerBankDetail> GetPowerBankDetailByIdAsync(int powerBankId);
        Task<MobileCoverDetail> GetMobileCoverByIdAsync(int mobileCoverId);
        Task<TabletDetail> GetTabletByIdAsync(int tabletDetailId);
        Task<FlashMemoryDetail> GetFlashMemoryByIdAsync(int flashMemoryId);
        Task<SpeakerDetail> GetSpeakerByIdAsync(int speakerId);
        Task<SmartWatchDetail> GetSmartWatchByIdAsync(int smartWatchId);
        Task<WristWatchDetail> GetWristWatchByIdAsync(int wristWatchId);
        Task<MemoryCardDetail> GetMemoryCardByIdAsync(int memoryCardId);
        Task<AUXDetail> GetAUXByIdAsync(int auxId);
        Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeAndHeadPhoneDetailId);
        Task<ProductGallery> GetProductGalleryAsync(int productId, string imageName);
        Task<ProductTypes> GetProductTypeByIdAsync(int productId);

        // detail of every product
        Task<ProductDetailViewModel> GetProductDetailAsync(int productId, string shopperProductColorId);

        Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(int productId, string shopperProductColorId);

        // productId , productName , sellerId
        Task<Tuple<int, string, string>> GetProductRedirectionByShortKeyAsync(string key);
        // get product types
        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId);
        Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataAsync(int productId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId);
        Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataAsync(int productId, string shopperId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataAsync(int productId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataAsync(int productId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataAsync(int productId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataAsync(int productId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataAsync(int productId);
        Task<AddOrEditAUXViewModel> GetTypeAUXProductDataAsync(int productId);

        // inserts
        Task<ResultTypes> AddMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail);
        Task<ResultTypes> AddLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail);
        Task<ResultTypes> AddPowerBankAsync(Domain.Entities.Product.Product product, PowerBankDetail powerBank);
        Task<ResultTypes> AddMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail);
        Task<ResultTypes> AddTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail);
        Task<ResultTypes> AddHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task<ResultTypes> AddSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail);
        Task<ResultTypes> AddFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail);
        Task<ResultTypes> AddSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail);
        Task<ResultTypes> AddWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail);
        Task<ResultTypes> AddMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail);
        Task<ResultTypes> AddAUXAsync(Domain.Entities.Product.Product product, AUXDetail auxDetail);
        Task AddProductGalleryAsync(ProductGallery productGallery);

        // remove
        Task<ResultTypes> RemoveProductAccessAsync(int productId);

        // update  Task<ResultTypes> EditProductGalleryAsync(ProductGallery productGallery);
        Task<ResultTypes> EditProductAsync(Domain.Entities.Product.Product product);
        Task<ResultTypes> DeleteProductGalleryAsync(ProductGallery productGallery);
        Task<ResultTypes> EditMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail);
        Task<ResultTypes> EditLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail);
        Task<ResultTypes> EditPowerBankAsync(Domain.Entities.Product.Product product, PowerBankDetail powerBank);
        Task<ResultTypes> EditMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail);
        Task<ResultTypes> EditTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail);
        Task<ResultTypes> EditHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task<ResultTypes> EditFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail);
        Task<ResultTypes> EditSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail);
        Task<ResultTypes> EditSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail);
        Task<ResultTypes> EditWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail);
        Task<ResultTypes> EditMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail);
        Task<ResultTypes> EditAUXAsync(Domain.Entities.Product.Product product, AUXDetail auxDetail);
        // validations 
        Task<bool> IsProductExistAsync(int productId);
        Task<bool> IsProductGalleriesCountValidAsync(int productId);
        Task<bool> IsShopperProductExistAsync(string shopperProductId);
        Task<bool> IsShopperProductExistAsync(string shopperId, int productId);

        #region Favorite Product

        // product , pageId , totalPages
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, string type = "all", string sortBy = "news", int pageId = 1, int take = 18);
        Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId);
        Task<FavoriteProductResultType> AddFavoriteProductAsync(string userId, string shopperProductColorId);
        Task RemoveFavoriteProductAsync(FavoriteProduct favoriteProduct);

        #endregion

        #region Question

        IEnumerable<Question> GetProductQuestions(int productId);

        #endregion

        #region chart

        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);

        #endregion

        #region brand

        IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId);
        IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId);

        #endregion

        #region store title

        IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId);

        #endregion
    }
}