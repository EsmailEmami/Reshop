using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product.ProductDetail;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IProductRepository
    {
        #region GetAll

        IAsyncEnumerable<ProductViewModel> GetProducts();
        IAsyncEnumerable<ProductViewModel> GetProductsWithPagination(string type, string sortBy, int skip = 0, int take = 24);

        Task<string> GetProductFirstPictureName(int productId);
        #endregion

        #region Get With Type

        IEnumerable<Entities.Product.Product> GetTypeMobileProducts();
        IEnumerable<Entities.Product.Product> GetTypeLaptopProducts();

        Task<Entities.Product.Product> GetProductWithTypeAsync(int productId, string type);

        Task<Entities.Product.Product> GetProductByShortKeyAsync(string key);

        #endregion

        #region Count

        Task<int> GetProductsCountWithTypeAsync(string type);
        Task<int> GetUserFavoriteProductsCountWithTypeAsync(string userId, string type);

        #endregion

        #region Get By Id

        Task<Entities.Product.Product> GetProductByIdAsync(int productId);
        Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId);
        Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId);
        Task<MobileCoverDetail> GetMobileCoverDetailByIdAsync(int mobileCoverId);
        Task<TabletDetail> GetTabletDetailByIdAsync(int tabletDetailId);
        Task<FlashMemoryDetail> GetFlashMemoryDetailByIdAsync(int flashMemoryId);
        Task<SpeakerDetail> GetSpeakerDetailByIdAsync(int speakerDetailId);
        Task<SmartWatchDetail> GetSmartWatchDetailByIdAsync(int smartWatchDetailId);
        Task<WristWatchDetail> GetWristWatchDetailByIdAsync(int wristWatchDetailId);
        Task<MemoryCardDetail> GetMemoryCardDetailByIdAsync(int memoryCardDetailId);
        Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeOrHeadPhoneDetailId);
        IAsyncEnumerable<ProductGallery> GetProductImages(int productId);


        Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId);
        Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId);
        Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId);
        Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId);
        Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(int productId);
        Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId);
        Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId);
        Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId);
        Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId);
        Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId);

        Task<string> GetProductTypeAsync(int productId);
        Task<ProductGallery> GetProductGalleryByIdAsync(string productGalleryId);
        Task<int> GetProductGalleriesCountByProductIdAsync(int productId);

        #endregion

        #region Validations

        Task<bool> IsProductExistAsync(int productId);
        Task<bool> IsProductExistByShortKeyAsync(string shortKey);

        #endregion

        #region Insert

        Task AddProductAsync(Entities.Product.Product product);
        Task AddMobileDetailAsync(MobileDetail mobileDetail);
        Task AddLaptopDetailAsync(LaptopDetail laptopDetail);
        Task AddProductGalley(ProductGallery productGallery);
        Task AddMobileCoverDetailAsync(MobileCoverDetail mobileCoverDetail);
        Task AddTabletDetailAsync(TabletDetail tabletDetail);
        Task AddHandsfreeAndHeadPhoneDetailAsync(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        Task AddFlashMemoryDetailAsync(FlashMemoryDetail flashMemoryDetail);
        Task AddSpeakerDetailAsync(SpeakerDetail speakerDetail);
        Task AddWristWatchDetailAsync(WristWatchDetail wristWatchDetail);
        Task AddSmartWatchAsync(SmartWatchDetail smartWatchDetail);
        Task AddMemoryCardDetailAsync(MemoryCardDetail memoryCardDetail);


        #endregion

        #region Update

        void UpdateProduct(Entities.Product.Product product);
        void UpdateMobileDetail(MobileDetail mobileDetail);
        void UpdateLaptopDetail(LaptopDetail laptopDetail);
        void UpdateMobileCoverDetail(MobileCoverDetail mobileCoverDetail);
        void UpdateTabletDetail(TabletDetail tabletDetail);
        void UpdateHandsfreeAndHeadPhoneDetail(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail);
        void UpdateFlashMemoryDetail(FlashMemoryDetail flashMemoryDetail);
        void UpdateSpeakerDetail(SpeakerDetail speakerDetail);
        void UpdateWristWatchDetail(WristWatchDetail wristWatchDetail);
        void UpdateSmartWatch(SmartWatchDetail smartWatchDetail);
        void UpdateMemoryCardDetail(MemoryCardDetail memoryCardDetail);

        #endregion



        #region Product

        void RemoveProduct(Entities.Product.Product product);
        void RemoveRangeProducts(List<Entities.Product.Product> products);

        #endregion

        void RemoveMobileDetail(MobileDetail mobileDetail);
        void RemoveRangeMobileDetails(List<MobileDetail> mobileDetails);

        void RemoveLaptopDetail(LaptopDetail laptopDetail);
        void RemoveRangeLaptopDetails(List<LaptopDetail> laptopDetails);

        void RemoveMobileCoverDetail(MobileCoverDetail mobileCoverDetail);


        #region Other

        IEnumerable<ChildCategory> GetProductChildCategories(int productId);
        IEnumerable<Comment> GetProductComments(int productId);
        IEnumerable<Question> GetProductQuestions(int productId);

        #endregion

        #region Favorite Products

        IAsyncEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string type, string sortBy, int skip = 0, int take = 24);
        Task AddToFavoriteProductAsync(FavoriteProduct favoriteProduct);
        void RemoveFavoriteProduct(FavoriteProduct favoriteProduct);
        Task<bool> IsFavoriteProductExistAsync(string favoriteProductId);
        Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId);

        #endregion

        #region Filter

        IAsyncEnumerable<string> GetProductsNameByFilter(string productName);
        IAsyncEnumerable<ProductViewModel> GetProductsByFilter(string productName);

        #endregion

        Task SaveChangesAsync();
    }
}