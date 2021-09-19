﻿using System;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.DTOs.Shopper;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IProductRepository
    {
        IEnumerable<ProductViewModel> GetProductsWithPagination(string type, string sortBy, int skip, int take);
        IEnumerable<ProductDataForAdmin> GetProductsWithPaginationForAdmin(string type, int skip, int take, string filter);
        IEnumerable<ProductViewModel> GetProductsOfCategoryWithPagination(int categoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<string> brands = null);
        IEnumerable<ProductViewModel> GetProductsOfChildCategoryWithPagination(int childCategoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<string> brands = null);
        Task<string> GetProductFirstPictureName(int productId);

        Task<ProductDataForDetailViewModel> GetProductDataForDetailAsync(string shopperProductColorId);

        IEnumerable<string> GetBrandsOfCategory(int categoryId);
        IEnumerable<string> GetBrandsOfChildCategory(int childCategoryId);

        IEnumerable<Entities.Product.Product> GetTypeMobileProducts();
        IEnumerable<Entities.Product.Product> GetTypeLaptopProducts();



        Task<Tuple<string, string>> GetProductRedirectionByShortKeyAsync(string key);
        Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId);
        Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId);
        IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId, int colorId);
        IEnumerable<Tuple<int, string, string, string>> GetProductColorsWithDetail(int productId);
        Task<Entities.Product.Product> GetProductByIdAsync(int productId);
        IEnumerable<Tuple<int, string>> GetProductColors(int productId);
        Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(string shopperProductColorId);

        Task<int> GetProductIdOfShopperProductColorIdAsync(string shopperProductColorId);

        #region shopper product

        // is shopperUser Id was NULL the query find best shopper automatically
        Task<ShopperProduct> GetProductWithTypeAsync(int productId, string type, string shopperId = "");

        IEnumerable<ProductViewModel> GetShopperProductsWithPagination(string shopperId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<string> brands = null);
        #endregion


        Task<int> GetProductsCountWithTypeAsync(string type);
        Task<int> GetUserFavoriteProductsCountWithTypeAsync(string userId, string type);
        Task<int> GetCategoryProductsCountWithTypeAsync(int categoryId, string type = "");
        Task<int> GetChildCategoryProductsCountWithTypeAsync(int childCategoryId, string type = "");




        Task<ShopperProduct> GetShopperProductAsync(string shopperId, int productId);
        Task<ShopperProduct> GetShopperProductAsync(string shopperProductId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductId, int colorId);
        Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductColorId);
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
        Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeOrHeadPhoneDetailId);
        IEnumerable<ProductGallery> GetProductImages(int productId);


        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId);
        Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataForEditAsync(int productId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId);
        Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(int productId, string shopperId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId);
        Task<AddOrEditAUXViewModel> GetTypeAUXProductDataForEditAsync(int productId);
        Task<string> GetProductTypeAsync(int productId);
        Task<ProductGallery> GetProductGalleryAsync(int productId, string imageName);
        Task<int> GetProductGalleriesCountByProductIdAsync(int productId);





        Task<bool> IsProductExistAsync(int productId);
        Task<bool> IsProductExistByShortKeyAsync(string shortKey);





        Task AddProductAsync(Entities.Product.Product product);
        Task AddMobileDetailAsync(MobileDetail mobileDetail);
        Task AddLaptopDetailAsync(LaptopDetail laptopDetail);
        Task AddPowerBankDetailAsync(PowerBankDetail powerBank);
        Task AddProductGalley(ProductGallery productGallery);
        Task AddMobileCoverDetailAsync(MobileCoverDetail mobileCoverDetail);
        Task AddTabletDetailAsync(TabletDetail tabletDetail);
        Task AddHandsfreeAndHeadPhoneDetailAsync(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task AddFlashMemoryDetailAsync(FlashMemoryDetail flashMemoryDetail);
        Task AddSpeakerDetailAsync(SpeakerDetail speakerDetail);
        Task AddWristWatchDetailAsync(WristWatchDetail wristWatchDetail);
        Task AddSmartWatchAsync(SmartWatchDetail smartWatchDetail);
        Task AddMemoryCardDetailAsync(MemoryCardDetail memoryCardDetail);
        Task AddAUXDetailAsync(AUXDetail auxDetail);




        void UpdateProduct(Entities.Product.Product product);
        void RemoveProductGallery(ProductGallery productGallery);
        void UpdateMobileDetail(MobileDetail mobileDetail);
        void UpdateLaptopDetail(LaptopDetail laptopDetail);
        void UpdatePowerBankDetail(PowerBankDetail powerBank);
        void UpdateMobileCoverDetail(MobileCoverDetail mobileCoverDetail);
        void UpdateTabletDetail(TabletDetail tabletDetail);
        void UpdateHandsfreeAndHeadPhoneDetail(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        void UpdateFlashMemoryDetail(FlashMemoryDetail flashMemoryDetail);
        void UpdateSpeakerDetail(SpeakerDetail speakerDetail);
        void UpdateWristWatchDetail(WristWatchDetail wristWatchDetail);
        void UpdateSmartWatch(SmartWatchDetail smartWatchDetail);
        void UpdateMemoryCardDetail(MemoryCardDetail memoryCardDetail);
        void UpdateAUXDetail(AUXDetail auxDetail);







        void RemoveProduct(Entities.Product.Product product);
        void RemoveRangeProducts(List<Entities.Product.Product> products);



        void RemoveMobileDetail(MobileDetail mobileDetail);
        void RemoveRangeMobileDetails(List<MobileDetail> mobileDetails);

        void RemoveLaptopDetail(LaptopDetail laptopDetail);
        void RemoveRangeLaptopDetails(List<LaptopDetail> laptopDetails);

        void RemoveMobileCoverDetail(MobileCoverDetail mobileCoverDetail);




        IEnumerable<ChildCategory> GetProductChildCategories(int productId);
        IEnumerable<Comment> GetProductComments(int productId);
        IEnumerable<Question> GetProductQuestions(int productId);





        IEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string type, string sortBy, int skip = 0, int take = 24);
        Task AddToFavoriteProductAsync(FavoriteProduct favoriteProduct);
        void RemoveFavoriteProduct(FavoriteProduct favoriteProduct);
        void UpdateFavoriteProduct(FavoriteProduct favoriteProduct);
        Task<bool> IsFavoriteProductExistAsync(string favoriteProductId);
        Task<bool> IsFavoriteProductExistAsync(string userId, string shopperProductColorId);
        Task<FavoriteProduct> GetFavoriteProductAsync(string favoriteProductId);
        Task<FavoriteProduct> GetFavoriteProductAsync(string userId, string shopperProductColorId);



        #region Chart

        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);

        #endregion

        #region store title

        IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId);

        #endregion

        #region brand

        Task<Brand> GetBrandByIdAsync(int brandId);
        IEnumerable<Tuple<int, string>> GetBrandsForShow();
        IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId);
        IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId);

        #endregion

        Task SaveChangesAsync();
    }
}