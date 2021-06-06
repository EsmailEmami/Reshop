using Reshop.Application.Enums.Product;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Interfaces.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Shopper;
using ProductViewModel = Reshop.Domain.DTOs.Product.ProductViewModel;

namespace Reshop.Application.Services.Product
{
    public class ProductService : IProductService
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IShopperRepository _shopperRepository;

        public ProductService(IProductRepository productRepository, IShopperRepository shopperRepository)
        {
            _productRepository = productRepository;
            _shopperRepository = shopperRepository;
        }

        #endregion

        public IEnumerable<ProductViewModel> GetProductsWithType(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int take = 18)
        {
            return _productRepository.GetProductsWithPagination(Fixer.FixedToString(type), Fixer.FixedToString(sortBy), 0, take) as IEnumerable<ProductViewModel>;
        }

        public async Task<Tuple<IAsyncEnumerable<ProductViewModel>, int, int>> GetProductsWithPaginationAsync(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetProductsCountWithTypeAsync(type.ToString());

            var products = _productRepository.GetProductsWithPagination(Fixer.FixedToString(type), Fixer.FixedToString(sortBy), skip, take);


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IAsyncEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetCategoryProductsWithPaginationAsync(int categoryId, ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetCategoryProductsCountWithTypeAsync(categoryId, Fixer.FixedToString(type));

            var products = _productRepository.GetProductsOfCategoryWithPagination(categoryId, Fixer.FixedToString(type), Fixer.FixedToString(sortBy), skip, take) as IEnumerable<ProductViewModel>;


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetShopperProductsWithPaginationAsync(string shopperUserId, ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetShopperProductsCountWithTypeAsync(shopperUserId, type.ToString());

            var products = (IEnumerable<ProductViewModel>)_productRepository.GetShopperProductsWthPagination(shopperUserId, Fixer.FixedText(type.ToString()), Fixer.FixedText(sortBy.ToString()), skip, take);


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

        public async Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId)
        {
            return await _productRepository.GetMobileDetailByIdAsync(mobileDetailId);
        }

        public async Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId)
        {
            return await _productRepository.GetLaptopDetailByIdAsync(laptopDetailId);
        }

        public async Task<MobileCoverDetail> GetMobileCoverByIdAsync(int mobileCoverId)
            =>
                await _productRepository.GetMobileCoverDetailByIdAsync(mobileCoverId);

        public async Task<TabletDetail> GetTabletByIdAsync(int tabletDetailId)
            =>
                await _productRepository.GetTabletDetailByIdAsync(tabletDetailId);

        public async Task<FlashMemoryDetail> GetFlashMemoryByIdAsync(int flashMemoryId)
            =>
                await _productRepository.GetFlashMemoryDetailByIdAsync(flashMemoryId);

        public async Task<SpeakerDetail> GetSpeakerByIdAsync(int speakerId)
            =>
                await _productRepository.GetSpeakerDetailByIdAsync(speakerId);

        public async Task<SmartWatchDetail> GetSmartWatchByIdAsync(int smartWatchId)
            =>
                await _productRepository.GetSmartWatchDetailByIdAsync(smartWatchId);

        public async Task<WristWatchDetail> GetWristWatchByIdAsync(int wristWatchId)
            =>
                await _productRepository.GetWristWatchDetailByIdAsync(wristWatchId);

        public async Task<MemoryCardDetail> GetMemoryCardByIdAsync(int memoryCardId)
            =>
                await _productRepository.GetMemoryCardDetailByIdAsync(memoryCardId);

        public async Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeAndHeadPhoneDetailId)
            =>
                await _productRepository.GetHandsfreeAndHeadPhoneDetailByIdAsync(handsfreeAndHeadPhoneDetailId);

        public async Task<ProductGallery> GetProductGalleryByIdAsync(string productGalleryId)
            =>
                await _productRepository.GetProductGalleryByIdAsync(productGalleryId);

        public async Task<ProductTypes> GetProductTypeByIdAsync(int productId)
        {
            var productType = await _productRepository.GetProductTypeAsync(productId);

            return productType switch
            {
                "Mobile" => ProductTypes.Mobile,
                "Laptop" => ProductTypes.Laptop,
                "Computer" => ProductTypes.Computer,
                "MobileCover" => ProductTypes.MobileCover,
                "LaptopCover" => ProductTypes.LaptopCover,
                "Tablet" => ProductTypes.Tablet,
                "Glass" => ProductTypes.Glass,
                "SmartWatch" => ProductTypes.SmartWatch,
                "WristWatch" => ProductTypes.WristWatch,
                "HeadPhone" => ProductTypes.HeadPhone,
                "Handsfree" => ProductTypes.Handsfree,
                "Speaker" => ProductTypes.Speaker,
                "PowerBank" => ProductTypes.PowerBank,
                "MobileBatteryCharger" => ProductTypes.MobileBatteryCharger,
                "LaptopBatteryCharger" => ProductTypes.LaptopBatteryCharger,
                "FlashMemory" => ProductTypes.FlashMemory,

                _ => ProductTypes.NotFound
            };
        }

        public async Task<ProductDetailViewModel> GetProductDetailAsync(int productId)
        {
            if (!await _productRepository.IsProductExistAsync(productId)) return null;

            string productType = await _productRepository.GetProductTypeAsync(productId);

            var product = await _productRepository.GetProductWithTypeAsync(productId, productType);

            var childCategories = _productRepository.GetProductChildCategories(productId);
            var comments = _productRepository.GetProductComments(productId);
            var questions = _productRepository.GetProductQuestions(productId);
            var productGalleries = _productRepository.GetProductImages(productId);
            var shoppers = _shopperRepository.GetProductShoppers(productId);

            return new ProductDetailViewModel()
            {
                ProductType = productType,
                Product = product,
                ChildCategories = childCategories,
                Comments = comments,
                Questions = questions,
                ProductGalleries = productGalleries,
                Shoppers = shoppers
            };
        }

        public async Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key)
            =>
                await _productRepository.GetProductByShortKeyAsync(key);


        public async Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeMobileProductDataForEditAsync(productId);

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeLaptopProductDataForEditAsync(productId);

        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeMobileCoverProductDataForEditAsync(productId);

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeTabletProductDataForEditAsync(productId);

        public async Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(productId);

        public async Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeFlashMemoryProductDataForEditAsync(productId);

        public async Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeSpeakerProductDataForEditAsync(productId);

        public async Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeWristWatchProductDataForEditAsync(productId);

        public async Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeSmartWatchProductDataForEditAsync(productId);

        public async Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeMemoryCardProductDataForEditAsync(productId);

        public async Task<ResultTypes> AddMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail)
        {
            try
            {
                await _productRepository.AddMobileDetailAsync(mobileDetail);
                await _productRepository.SaveChangesAsync();

                product.MobileDetailId = mobileDetail.MobileDetailId;
                product.MobileDetail = mobileDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task RemoveLaptopAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);


            if (product.LaptopDetailId != null)
            {
                var laptopDetail = await _productRepository.GetLaptopDetailByIdAsync(product.LaptopDetailId.Value);
                _productRepository.RemoveLaptopDetail(laptopDetail);
            }
            _productRepository.RemoveProduct(product);

            await _productRepository.SaveChangesAsync();
        }

        public async Task<ResultTypes> EditMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail)
        {
            try
            {
                await _productRepository.AddMobileDetailAsync(mobileDetail);
                await _productRepository.SaveChangesAsync();

                product.MobileDetailId = mobileDetail.MobileDetailId;
                product.MobileDetail = mobileDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail)
        {
            try
            {
                await _productRepository.AddLaptopDetailAsync(laptopDetail);
                await _productRepository.SaveChangesAsync();

                product.LaptopDetailId = laptopDetail.LaptopDetailId;
                product.LaptopDetail = laptopDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail)
        {
            try
            {
                await _productRepository.AddMobileCoverDetailAsync(mobileCoverDetail);
                await _productRepository.SaveChangesAsync();

                product.MobileCoverDetailId = mobileCoverDetail.MobileCoverDetailId;
                product.MobileCoverDetail = mobileCoverDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail)
        {
            try
            {
                await _productRepository.AddTabletDetailAsync(tabletDetail);
                await _productRepository.SaveChangesAsync();

                product.TabletDetailId = tabletDetail.TabletDetailId;
                product.TabletDetail = tabletDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail)
        {
            try
            {
                await _productRepository.AddHandsfreeAndHeadPhoneDetailAsync(handsfreeAndHeadPhoneDetail);
                await _productRepository.SaveChangesAsync();

                product.HandsfreeAndHeadPhoneDetailId = handsfreeAndHeadPhoneDetail.HeadPhoneDetailId;
                product.HandsfreeAndHeadPhoneDetail = handsfreeAndHeadPhoneDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail)
        {
            try
            {
                await _productRepository.AddSpeakerDetailAsync(speakerDetail);
                await _productRepository.SaveChangesAsync();

                product.SpeakerDetailId = speakerDetail.SpeakerDetailId;
                product.SpeakerDetail = speakerDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail)
        {
            try
            {
                await _productRepository.AddFlashMemoryDetailAsync(flashMemoryDetail);
                await _productRepository.SaveChangesAsync();

                product.FlashMemoryDetailId = flashMemoryDetail.FlashDetailId;
                product.FlashMemoryDetail = flashMemoryDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail)
        {
            try
            {
                await _productRepository.AddSmartWatchAsync(smartWatchDetail);
                await _productRepository.SaveChangesAsync();

                product.SmartWatchDetailId = smartWatchDetail.SmartWatchDetailId;
                product.SmartWatchDetail = smartWatchDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail)
        {
            try
            {
                await _productRepository.AddWristWatchDetailAsync(wristWatchDetail);
                await _productRepository.SaveChangesAsync();

                product.WristWatchDetailId = wristWatchDetail.WristWatchDetailId;
                product.WristWatchDetail = wristWatchDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail)
        {
            try
            {
                await _productRepository.AddMemoryCardDetailAsync(memoryCardDetail);
                await _productRepository.SaveChangesAsync();

                product.MemoryCardDetailId = memoryCardDetail.MemoryCardDetailId;
                product.MemoryCardDetail = memoryCardDetail;

                while (await _productRepository.IsProductExistByShortKeyAsync(product.ShortKey))
                {
                    product.ShortKey = NameGenerator.GenerateShortKey();
                }

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task AddProductGalleryAsync(ProductGallery productGallery)
        {
            await _productRepository.AddProductGalley(productGallery);
            await _productRepository.SaveChangesAsync();
        }

        public async Task RemoveMobileAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product.MobileDetailId != null)
            {
                var mobileDetail = await _productRepository.GetMobileDetailByIdAsync(product.MobileDetailId.Value);
                _productRepository.RemoveMobileDetail(mobileDetail);
            }
            _productRepository.RemoveProduct(product);


            await _productRepository.SaveChangesAsync();
        }

        public async Task<ResultTypes> EditLaptopAsync(Domain.Entities.Product.Product product, LaptopDetail laptopDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                {
                    return ResultTypes.Failed;
                }
                else
                {
                    _productRepository.UpdateLaptopDetail(laptopDetail);
                    _productRepository.UpdateProduct(product);

                    await _productRepository.SaveChangesAsync();

                    return ResultTypes.Successful;
                }
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditMobileCoverAsync(Domain.Entities.Product.Product product, MobileCoverDetail mobileCoverDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                {
                    return ResultTypes.Failed;
                }
                else
                {
                    _productRepository.UpdateMobileCoverDetail(mobileCoverDetail);
                    _productRepository.UpdateProduct(product);

                    await _productRepository.SaveChangesAsync();

                    return ResultTypes.Successful;
                }
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditTabletAsync(Domain.Entities.Product.Product product, TabletDetail tabletDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                {
                    return ResultTypes.Failed;
                }
                else
                {
                    _productRepository.UpdateTabletDetail(tabletDetail);
                    _productRepository.UpdateProduct(product);

                    await _productRepository.SaveChangesAsync();

                    return ResultTypes.Successful;
                }
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditHandsfreeAndHeadPhoneDetailAsync(Domain.Entities.Product.Product product, HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateHandsfreeAndHeadPhoneDetail(handsfreeAndHeadPhoneDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;

            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditFlashMemoryAsync(Domain.Entities.Product.Product product, FlashMemoryDetail flashMemoryDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateFlashMemoryDetail(flashMemoryDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;

            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditSpeakerAsync(Domain.Entities.Product.Product product, SpeakerDetail speakerDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateSpeakerDetail(speakerDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;

            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditSmartWatchAsync(Domain.Entities.Product.Product product, SmartWatchDetail smartWatchDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateSmartWatch(smartWatchDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;

            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditWristWatchAsync(Domain.Entities.Product.Product product, WristWatchDetail wristWatchDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateWristWatchDetail(wristWatchDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditMemoryCardAsync(Domain.Entities.Product.Product product, MemoryCardDetail memoryCardDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateMemoryCardDetail(memoryCardDetail);
                _productRepository.UpdateProduct(product);

                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsProductExistAsync(int productId)
        {
            return await _productRepository.IsProductExistAsync(productId);
        }

        public async Task<bool> IsProductGalleriesCountValidAsync(int productId)
        {
            var galleryCount = await _productRepository.GetProductGalleriesCountByProductIdAsync(productId);

            if (galleryCount < 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Tuple<IAsyncEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetUserFavoriteProductsCountWithTypeAsync(userId, Fixer.FixedToString(type));

            var products = _productRepository.GetProductsWithPagination(type.ToString(), sortBy.ToString(), skip, take);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IAsyncEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId)
        {
            return await _productRepository.GetFavoriteProductByIdAsync(favoriteProductId);
        }

        public async Task AddFavoriteProductAsync(FavoriteProduct favoriteProduct)
        {
            await _productRepository.AddToFavoriteProductAsync(favoriteProduct);
            await _productRepository.SaveChangesAsync();

        }

        public async Task RemoveFavoriteProductAsync(FavoriteProduct favoriteProduct)
        {
            _productRepository.RemoveFavoriteProduct(favoriteProduct);
            await _productRepository.SaveChangesAsync();
        }
    }
}