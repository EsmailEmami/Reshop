﻿using Reshop.Application.Enums.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.Entities.Product.ProductDetail;

namespace Reshop.Application.Interfaces.Product
{
    public interface IProductService
    {

        IAsyncEnumerable<ProductViewModel> GetProductsWithType(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int take = 18);

        // product , pageId , totalPages
        Task<Tuple<IAsyncEnumerable<ProductViewModel>, int, int>> GetProductsWithPaginationAsync(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18);

        // get by id 
        Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId);
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
        Task<ProductDetailViewModel> GetProductDetailAsync(int productId);
        Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key);

        // get product types
        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId);
        Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataAsync(int productId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataAsync(int productId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataAsync(int productId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataAsync(int productId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataAsync(int productId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataAsync(int productId);

        // insert
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
        Task RemoveMobileAsync(int productId);
        Task RemoveLaptopAsync(int productId);

        // update
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

        #region Favorite Product

        // product , pageId , totalPages
        Task<Tuple<IAsyncEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18);
        Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId);
        Task AddFavoriteProductAsync(FavoriteProduct favoriteProduct);
        Task RemoveFavoriteProductAsync(FavoriteProduct favoriteProduct);

        #endregion
    }
}