﻿using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;

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
            return _productRepository.GetProductsWithPagination(Fixer.FixedToString(type), Fixer.FixedToString(sortBy), 0, take);
        }

        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetProductsWithPaginationAsync(string type = "all", string sortBy = "news", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetProductsCountWithTypeAsync(type.FixedText());

            var products = _productRepository.GetProductsWithPagination(type.FixedText(), sortBy.FixedText(), skip, take);


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<Tuple<IEnumerable<ProductDataForAdmin>, int, int>> GetProductsWithPaginationForAdminAsync(string type = "all", int pageId = 1, int take = 18, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetProductsCountWithTypeAsync(type.FixedText());

            var products = _productRepository.GetProductsWithPaginationForAdmin(type.FixedText(), skip, take, filter);


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IEnumerable<ProductDataForAdmin>, int, int>(products, pageId, totalPages);
        }

        public async Task<CategoryOrChildCategoryProductsForShow> GetCategoryProductsWithPaginationAsync(int categoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4


            int productsCount = await _productRepository.GetCategoryProductsCountWithTypeAsync(categoryId);

            var products = _productRepository.GetProductsOfCategoryWithPagination(categoryId, Fixer.FixedText(sortBy), skip, take, filter, minPrice != null ? decimal.Parse(minPrice) : 0, maxPrice != null ? decimal.Parse(maxPrice) : 0, brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            IEnumerable<string> brandsShow = _productRepository.GetBrandsOfCategory(categoryId);


            return new CategoryOrChildCategoryProductsForShow()
            {
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow
            };
        }

        public async Task<CategoryOrChildCategoryProductsForShow> GetChildCategoryProductsWithPaginationAsync(int childCategoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4


            int productsCount = await _productRepository.GetChildCategoryProductsCountWithTypeAsync(childCategoryId);

            var products = _productRepository.GetProductsOfChildCategoryWithPagination(childCategoryId, sortBy.FixedText(), skip, take, filter, minPrice != null ? decimal.Parse(minPrice) : 0, maxPrice != null ? decimal.Parse(maxPrice) : 0, brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            IEnumerable<string> brandsShow = _productRepository.GetBrandsOfChildCategory(childCategoryId);

            return new CategoryOrChildCategoryProductsForShow()
            {
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow
            };
        }

        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetShopperProductsWithPaginationAsync(string shopperId, string type = "all", string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<string> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _shopperRepository.GetShopperProductsCountWithTypeAsync(shopperId, type.FixedText());

            var products = _productRepository.GetShopperProductsWithPagination(shopperId, sortBy.FixedText(), skip, take, filter, minPrice != null ? decimal.Parse(minPrice) : 0, maxPrice != null ? decimal.Parse(maxPrice) : 0, brands);


            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<int> GetProductsCountWithTypeAsync(string type = "all")
            => await _productRepository.GetProductsCountWithTypeAsync(type);

        public async Task<ProductDetailForShow> GetProductDetailForShopperAsync(int productId, string shopperId)
        {
            string shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId == null)
            {
                return null;
            }

            return await _productRepository.GetProductDetailForShopperAsync(shopperProductId);
        }

        public async Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId) =>
            await _productRepository.GetProductDetailForShopperAsync(shopperProductId);

        public async Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId)
        {
            var model = await _productRepository.GetProductDetailForAdminAsync(productId);
            model.Colors = _productRepository.GetProductColors(model.ProductId);

            return model;
        }


        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId) =>
            await _productRepository.GetProductByIdAsync(productId);

        public async Task<ShopperProduct> GetShopperProductAsync(int productId, string shopperId) =>
            await _productRepository.GetShopperProductAsync(shopperId, productId);

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperProductId) =>
            await _productRepository.GetShopperProductAsync(shopperProductId);

        public async Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(int productId, string shopperId, int colorId)
        {
            string shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId is null)
                return null;

            if (!await _shopperRepository.IsShopperProductColorExistAsync(shopperProductId, colorId))
                return null;

            return await _productRepository.GetShopperProductColorForEditAsync(shopperProductId, colorId);
        }

        public async Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductColorId) =>
            await _productRepository.GetShopperProductColorForEditAsync(shopperProductColorId);

        public async Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId)
        {
            return await _productRepository.GetMobileDetailByIdAsync(mobileDetailId);
        }

        public async Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId)
        {
            return await _productRepository.GetLaptopDetailByIdAsync(laptopDetailId);
        }

        public async Task<PowerBankDetail> GetPowerBankDetailByIdAsync(int powerBankId)
        {
            return await _productRepository.GetPowerBankDetailByIdAsync(powerBankId);
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

        public async Task<AUXDetail> GetAUXByIdAsync(int auxId) =>
            await _productRepository.GetAUXByIdAsync(auxId);

        public async Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeAndHeadPhoneDetailId)
            =>
                await _productRepository.GetHandsfreeAndHeadPhoneDetailByIdAsync(handsfreeAndHeadPhoneDetailId);

        public async Task<ProductGallery> GetProductGalleryAsync(int productId, string imageName)
            =>
                await _productRepository.GetProductGalleryAsync(productId, imageName);

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
                "AUX" => ProductTypes.AUX,

                _ => ProductTypes.NotFound,
            };
        }

        public async Task<ProductDetailViewModel> GetProductDetailAsync(int productId, string shopperProductColorId)
        {
            var product = await _productRepository.GetProductDataForDetailAsync(shopperProductColorId);

            var childCategories = _productRepository.GetProductChildCategories(productId);
            var comments = _productRepository.GetProductComments(productId);
            var productGalleries = _productRepository.GetProductImages(productId);
            var shoppers = _productRepository.GetProductShoppers(productId, product.SelectedColor);
            var colors = _productRepository.GetProductColorsWithDetail(productId);


            return new ProductDetailViewModel()
            {
                Product = product,
                ChildCategories = childCategories,
                Comments = comments,
                ProductGalleries = productGalleries,
                Shoppers = shoppers,
                Colors = colors
            };
        }

        public async Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(int productId, string shopperProductColorId) =>
            await _productRepository.EditProductDetailShopperAsync(productId, shopperProductColorId);

        public async Task<Tuple<int, string, string>> GetProductRedirectionByShortKeyAsync(string key)
            =>
                await _productRepository.GetProductRedirectionByShortKeyAsync(key);

        public async Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeMobileProductDataForEditAsync(productId);

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeLaptopProductDataForEditAsync(productId);

        public async Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataAsync(int productId)
            =>
                await _productRepository.GetTypePowerBankProductDataForEditAsync(productId);

        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeMobileCoverProductDataForEditAsync(productId);

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeTabletProductDataForEditAsync(productId);

        public async Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataAsync(int productId, string shopperId)
            =>
                await _productRepository.GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(productId, shopperId);

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

        public async Task<AddOrEditAUXViewModel> GetTypeAUXProductDataAsync(int productId)
            =>
                await _productRepository.GetTypeAUXProductDataForEditAsync(productId);

        public async Task<ResultTypes> AddMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail)
        {
            try
            {
                await _productRepository.AddMobileDetailAsync(mobileDetail);
                await _productRepository.SaveChangesAsync();

                product.MobileDetailId = mobileDetail.MobileDetailId;
                product.MobileDetail = mobileDetail;

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }


        public async Task<ResultTypes> EditProductAsync(Domain.Entities.Product.Product product)
        {
            try
            {
                _productRepository.UpdateProduct(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteProductGalleryAsync(ProductGallery productGallery)
        {
            try
            {
                _productRepository.RemoveProductGallery(productGallery);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditMobileAsync(Domain.Entities.Product.Product product, MobileDetail mobileDetail)
        {
            try
            {
                await _productRepository.AddMobileDetailAsync(mobileDetail);
                await _productRepository.SaveChangesAsync();

                product.MobileDetailId = mobileDetail.MobileDetailId;
                product.MobileDetail = mobileDetail;

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

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddPowerBankAsync(Domain.Entities.Product.Product product, PowerBankDetail powerBank)
        {
            try
            {
                await _productRepository.AddPowerBankDetailAsync(powerBank);
                await _productRepository.SaveChangesAsync();

                product.PowerBankDetailId = powerBank.PowerBankId;
                product.PowerBankDetail = powerBank;

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

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddAUXAsync(Domain.Entities.Product.Product product, AUXDetail auxDetail)
        {
            try
            {
                await _productRepository.AddAUXDetailAsync(auxDetail);
                await _productRepository.SaveChangesAsync();

                product.AuxDetailId = auxDetail.AUXDetailId;
                product.AuxDetail = auxDetail;

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

        public async Task<ResultTypes> RemoveProductAccessAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product is null)
            {
                return ResultTypes.Failed;
            }

            product.IsActive = false;

            await _productRepository.SaveChangesAsync();

            return ResultTypes.Successful;
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

        public async Task<ResultTypes> EditPowerBankAsync(Domain.Entities.Product.Product product, PowerBankDetail powerBank)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                {
                    return ResultTypes.Failed;
                }
                else
                {
                    _productRepository.UpdatePowerBankDetail(powerBank);
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

        public async Task<ResultTypes> EditAUXAsync(Domain.Entities.Product.Product product, AUXDetail auxDetail)
        {
            try
            {
                if (!await _productRepository.IsProductExistAsync(product.ProductId))
                    return ResultTypes.Failed;


                _productRepository.UpdateAUXDetail(auxDetail);
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

        public async Task<bool> IsShopperProductExistAsync(string shopperProductId) =>
            await _shopperRepository.IsShopperProductExistAsync(shopperProductId);

        public async Task<bool> IsShopperProductExistAsync(string shopperId, int productId) =>
            await _shopperRepository.IsShopperProductExistAsync(shopperId, productId);


        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, string type = "all", string sortBy = "news", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetUserFavoriteProductsCountWithTypeAsync(userId, type.FixedText());

            var products = _productRepository.GetUserFavoriteProductsWithPagination(userId, type.FixedText(), sortBy.FixedText(), skip, take);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId)
        {
            return await _productRepository.GetFavoriteProductAsync(favoriteProductId);
        }

        public async Task<FavoriteProductResultType> AddFavoriteProductAsync(string userId, string shopperProductColorId)
        {
            if (!await _shopperRepository.IsShopperProductColorExistAsync(shopperProductColorId))
                return FavoriteProductResultType.NotFound;

            try
            {
                var favoriteProduct = await _productRepository.GetFavoriteProductAsync(userId, shopperProductColorId);
                if (favoriteProduct != null)
                {
                    if (favoriteProduct.ShopperProductColorId == shopperProductColorId)
                    {
                        return FavoriteProductResultType.Available;
                    }
                    else
                    {
                        favoriteProduct.ShopperProductColorId = shopperProductColorId;

                        _productRepository.UpdateFavoriteProduct(favoriteProduct);
                        await _productRepository.SaveChangesAsync();

                        return FavoriteProductResultType.ProductReplaced;
                    }
                }
                else
                {
                    var newFavoriteProduct = new FavoriteProduct()
                    {
                        UserId = userId,
                        ShopperProductColorId = shopperProductColorId,
                    };


                    await _productRepository.AddToFavoriteProductAsync(newFavoriteProduct);
                    await _productRepository.SaveChangesAsync();
                    return FavoriteProductResultType.Successful;
                }
            }
            catch
            {
                return FavoriteProductResultType.NotFound;
            }

        }

        public async Task RemoveFavoriteProductAsync(FavoriteProduct favoriteProduct)
        {
            _productRepository.RemoveFavoriteProduct(favoriteProduct);
            await _productRepository.SaveChangesAsync();
        }

        public IEnumerable<Question> GetProductQuestions(int productId) =>
            _productRepository.GetProductQuestions(productId);

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId) =>
            _productRepository.GetLastThirtyDayProductDataChart(productId);

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId,
            int colorId) => _productRepository.GetLastThirtyDayColorProductDataChart(productId, colorId);

        public IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId) =>
            _productRepository.GetColorsOfProductDataChart(productId);

        public IEnumerable<Tuple<int, string>> GetBrandOfficialProducts(int brandId) =>
            _productRepository.GetBrandOfficialProducts(brandId);

        public IEnumerable<Tuple<int, string>> GetProductsOfOfficialProduct(int officialProductId) =>
            _productRepository.GetProductsOfOfficialProduct(officialProductId);

        public IEnumerable<Tuple<int, string>> GetBrandsOfStoreTitle(int storeTitleId) =>
            _productRepository.GetBrandsOfStoreTitle(storeTitleId);
    }
}