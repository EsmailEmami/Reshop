﻿using Reshop.Application.Enums.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Application.Interfaces.Product
{
    public interface IProductService
    {

        IEnumerable<ProductViewModel> GetProductsWithType(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int take = 18);

        // product , pageId , totalPages
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetProductsWithPaginationAsync(string type = "all", string sortBy = "news", int pageId = 1, int take = 18);
        Task<CategoryOrChildCategoryProductsForShow> GetCategoryProductsWithPaginationAsync(int categoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null);
        Task<CategoryOrChildCategoryProductsForShow> GetChildCategoryProductsWithPaginationAsync(int childCategoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null);
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetShopperProductsWithPaginationAsync(string shopperUserId, ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18);

        // get by id 
        Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId);
        Task<ShopperProduct> GetShopperProductAsync(int productId, string shopperUserId);
        Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId);
        Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId);
        Task<MobileCoverDetail> GetMobileCoverByIdAsync(int mobileCoverId);
        Task<TabletDetail> GetTabletByIdAsync(int tabletDetailId);
        Task<FlashMemoryDetail> GetFlashMemoryByIdAsync(int flashMemoryId);
        Task<SpeakerDetail> GetSpeakerByIdAsync(int speakerId);
        Task<SmartWatchDetail> GetSmartWatchByIdAsync(int smartWatchId);
        Task<WristWatchDetail> GetWristWatchByIdAsync(int wristWatchId);
        Task<MemoryCardDetail> GetMemoryCardByIdAsync(int memoryCardId);
        Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeAndHeadPhoneDetailId);
        Task<ProductGallery> GetProductGalleryByIdAsync(string productGalleryId);
        Task<ProductTypes> GetProductTypeByIdAsync(int productId);

        // detail of every product
        Task<ProductDetailViewModel> GetProductDetailAsync(int productId, string shopperUserId);
        Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key);

        // get product types
        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataAsync(int productId, string shopperUserId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataAsync(int productId, string shopperUserId);

        // inserts
        Task<ResultTypes> AddMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail);
        Task<ResultTypes> AddLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail);
        Task<ResultTypes> AddMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail);
        Task<ResultTypes> AddTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail);
        Task<ResultTypes> AddHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task<ResultTypes> AddSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail);
        Task<ResultTypes> AddFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail);
        Task<ResultTypes> AddSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail);
        Task<ResultTypes> AddWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail);
        Task<ResultTypes> AddMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail);
        Task AddProductGalleryAsync(ProductGallery productGallery);

        // remove
        Task<ResultTypes> RemoveMobileAsync(int productId);
        Task RemoveLaptopAsync(int productId);

        // update
        Task<ResultTypes> EditProductAsync(Domain.Entities.Product.Product product);
        Task<ResultTypes> EditMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail);
        Task<ResultTypes> EditLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail);
        Task<ResultTypes> EditMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail);
        Task<ResultTypes> EditTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail);
        Task<ResultTypes> EditHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task<ResultTypes> EditFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail);
        Task<ResultTypes> EditSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail);
        Task<ResultTypes> EditSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail);
        Task<ResultTypes> EditWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail);
        Task<ResultTypes> EditMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail);
        // validations 
        Task<bool> IsProductExistAsync(int productId);
        Task<bool> IsProductGalleriesCountValidAsync(int productId);

        Task<ResultTypes> AddUserProductViewAsync(UserProductView userProductView);
        Task<bool> IsUserProductViewExistAsync(int productId, string userIP);

        #region Favorite Product

        // product , pageId , totalPages
        Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, string type = "all", string sortBy = "news", int pageId = 1, int take = 18);
        Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId);
        Task<FavoriteProductResultType> AddFavoriteProductAsync(string userId, int productId, string shopperUserId);
        Task RemoveFavoriteProductAsync(FavoriteProduct favoriteProduct);

        #endregion
    }
}