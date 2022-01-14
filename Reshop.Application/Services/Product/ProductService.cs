using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Category;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Category;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Domain.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product.Options;
using OperatingSystem = Reshop.Domain.Entities.Product.Options.OperatingSystem;

namespace Reshop.Application.Services.Product
{
    public class ProductService : IProductService
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IShopperRepository _shopperRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;

        public ProductService(IProductRepository productRepository, IShopperRepository shopperRepository, ICartRepository cartRepository, IBrandRepository brandRepository, IColorRepository colorRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository)
        {
            _productRepository = productRepository;
            _shopperRepository = shopperRepository;
            _cartRepository = cartRepository;
            _brandRepository = brandRepository;
            _colorRepository = colorRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        #endregion

        public IEnumerable<ProductViewModel> GetProductsForShow(string type = "all", string sortBy = "news", int take = 18, List<int> brands = null) =>
            _productRepository.GetProductsWithPagination(type.FixedText(), sortBy.FixedText(), take: take, brands: brands);

        public async Task<Tuple<IEnumerable<ProductDataForAdmin>, int, int>> GetProductsWithPaginationForAdminAsync(string type = "all", int pageId = 1, int take = 18, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetProductsCountWithTypeAsync(type.FixedText());

            var products = _productRepository.GetProductsWithPaginationForAdmin(type.FixedText(), skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            return new Tuple<IEnumerable<ProductDataForAdmin>, int, int>(products, pageId, totalPages);
        }

        public async Task<CategoryProductsForShow> GetCategoryProductsWithPaginationAsync(int categoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<int> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4


            int productsCount = await _productRepository.GetCategoryProductsCountAsync(categoryId, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            var products = _productRepository.GetProductsOfCategoryWithPagination(categoryId, sortBy.FixedText(), skip, take, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            var brandsShow = _brandRepository.GetBrandsOfCategory(categoryId);

            decimal productMaxPrice = await _productRepository.GetMaxPriceOfCategoryProductsAsync(categoryId, filter, brands);
            decimal productMinPrice = await _productRepository.GetMinPriceOfCategoryProductsAsync(categoryId, filter, brands);

            return new CategoryProductsForShow()
            {
                CategoryId = categoryId,
                CategoryName = category.CategoryTitle,
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow,
                ProductsMaxPrice = productMaxPrice,
                ProductsMinPrice = productMinPrice
            };
        }

        public async Task<ProductsForShow> GetProductsWithPaginationAsync(string type = "all", string sortBy = "news", int pageId = 1, int take = 18,
            string filter = null, string minPrice = null, string maxPrice = null, List<int> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetProductsCountAsync(type.FixedText(), filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            var products = _productRepository.GetProductsWithPagination(type.FixedText(), sortBy.FixedText(), skip, take, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            var brandsShow = _brandRepository.GetBrandsOfProducts(type.FixedText());

            decimal productMaxPrice = await _productRepository.GetMaxPriceOfProductsAsync(type.FixedText(), filter, brands);
            decimal productMinPrice = await _productRepository.GetMinPriceOfProductsAsync(type.FixedText(), filter, brands);

            return new ProductsForShow()
            {
                Type = type,
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow,
                ProductsMaxPrice = productMaxPrice,
                ProductsMinPrice = productMinPrice
            };
        }

        public async Task<ChildCategoryProductsForShow> GetChildCategoryProductsWithPaginationAsync(int childCategoryId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<int> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4


            int productsCount = await _productRepository.GetChildCategoryProductsCountAsync(childCategoryId, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            var childCategory = await _categoryRepository.GetChildCategoryByIdAsync(childCategoryId);

            var category = await _categoryRepository.GetCategoryByIdAsync(childCategory.CategoryId);

            var products = _productRepository.GetProductsOfChildCategoryWithPagination(childCategoryId, sortBy.FixedText(), skip, take, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            var brandsShow = _brandRepository.GetBrandsOfChildCategory(childCategoryId);

            decimal productMaxPrice = await _productRepository.GetMaxPriceOfChildCategoryProductsAsync(childCategoryId, filter, brands);
            decimal productMinPrice = await _productRepository.GetMinPriceOfChildCategoryProductsAsync(childCategoryId, filter, brands);

            return new ChildCategoryProductsForShow()
            {
                ChildCategoryId = childCategoryId,
                ChildCategoryName = childCategory.ChildCategoryTitle,
                Category = new Tuple<int, string>(category.CategoryId, category.CategoryTitle),
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow,
                ProductsMaxPrice = productMaxPrice,
                ProductsMinPrice = productMinPrice
            };
        }

        public async Task<ShopperProductsForShow> GetShopperProductsWithPaginationAsync(string shopperId, string sortBy = "news", int pageId = 1, int take = 18,
            string filter = null, string minPrice = null, string maxPrice = null, List<int> brands = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetShopperProductsCountAsync(shopperId, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            var products = _productRepository.GetProductsOfShopperWithPagination(shopperId, sortBy.FixedText(), skip, take, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), brands);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            var brandsShow = _brandRepository.GetBrandsOfShopper(shopperId);

            decimal productMaxPrice = await _productRepository.GetMaxPriceOfShopperProductsAsync(shopperId, filter, brands);
            decimal productMinPrice = await _productRepository.GetMinPriceOfShopperProductsAsync(shopperId, filter, brands);

            var shopperStoreName = await _shopperRepository.GetShopperStoreNameAsync(shopperId);

            return new ShopperProductsForShow()
            {
                ShopperId = shopperId,
                StoreName = shopperStoreName,
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                Brands = brandsShow,
                ProductsMaxPrice = productMaxPrice,
                ProductsMinPrice = productMinPrice
            };
        }

        public async Task<BrandProductsForShow> GetBrandProductsWithPaginationAsync(int brandId, string sortBy = "news", int pageId = 1, int take = 18, string filter = null, string minPrice = null, string maxPrice = null, List<int> officialBrandProducts = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetBrandProductsCountAsync(brandId, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), officialBrandProducts);

            var products = _productRepository.GetProductsOfBrandWithPagination(brandId, sortBy.FixedText(), skip, take, filter, minPrice.ToDecimal(), maxPrice.ToDecimal(), officialBrandProducts);

            var brand = await _brandRepository.GetBrandByIdAsync(brandId);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            var officialBrandProductsForShow = _brandRepository.GetFullBrandOfficialProducts(brandId);

            decimal productMaxPrice = await _productRepository.GetMaxPriceOfBrandProductsAsync(brandId, filter, officialBrandProducts);
            decimal productMinPrice = await _productRepository.GetMinPriceOfBrandProductsAsync(brandId, filter, officialBrandProducts);


            return new BrandProductsForShow()
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                Products = products,
                PageId = pageId,
                TotalPages = totalPages,
                OfficialBrandProducts = officialBrandProductsForShow,
                ProductsMaxPrice = productMaxPrice,
                ProductsMinPrice = productMinPrice
            };
        }


        public async Task<ProductsGeneralDataForAdmin> GetProductsGeneralDataForAdminAsync()
        {
            int allProductsCount = await _productRepository.GetProductsCountWithTypeAsync();
            int activeProductsCount = await _productRepository.GetProductsCountWithTypeAsync("active");
            int nonActiveProductsCount = await _productRepository.GetProductsCountWithTypeAsync("non-active");
            int sellsCount = await _cartRepository.GetSellsCountFromDateAsync(DateTime.Now.AddDays(-30));

            var model = new ProductsGeneralDataForAdmin(
                allProductsCount,
                sellsCount,
                activeProductsCount,
                nonActiveProductsCount);

            return model;
        }

        public IEnumerable<SearchProductViewModel> SearchProducts(string filter) =>
             _productRepository.SearchProducts(filter);

        public async Task<ProductDetailForShow> GetProductDetailForShopperAsync(int productId, string shopperId)
        {
            string shopperProductId = await _shopperRepository.GetShopperProductIdAsync(shopperId, productId);

            if (shopperProductId == null)
            {
                return null;
            }

            var productDetail = await _productRepository.GetProductDetailForShopperAsync(shopperProductId);
            productDetail.ProductScore = await _commentRepository.GetCommentsScoreOfProductDetailAsync(productDetail.ProductId);


            return productDetail;
        }

        public async Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId) =>
            await _productRepository.GetProductDetailForShopperAsync(shopperProductId);

        public async Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId)
        {
            var productDetail = await _productRepository.GetProductDetailForAdminAsync(productId);
            productDetail.Colors = _colorRepository.GetProductColors(productDetail.ProductId);
            productDetail.ProductScore = await _commentRepository.GetCommentsScoreOfProductDetailAsync(productDetail.ProductId);


            return productDetail;
        }

        public async Task<ProductDataForCompareViewModel> GetProductDataForCompareAsync(int productId) =>
            await _productRepository.GetProductDataForCompareAsync(productId);

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

        public async Task<ProductDetailViewModel> GetProductDetailAsync(string shopperProductColorId)
        {
            var product = await _productRepository.GetProductDataForDetailAsync(shopperProductColorId);

            if (product == null)
                return null;

            var childCategory = await _categoryRepository.GetProductChildCategoryAsync(product.ProductId);
            var category = await _categoryRepository.GetProductCategoryAsync(product.ProductId);
            var comments = await _commentRepository.GetCommentsOfProductDetailAsync(product.ProductId);
            var productGalleries = _productRepository.GetProductImages(product.ProductId);
            var shoppers = _productRepository.GetProductShoppers(product.ProductId, product.SelectedColor);
            var colors = _colorRepository.GetProductColorsWithDetail(product.ProductId);

            var shopper = await _shopperRepository.GetShopperIdAndStoreNameOfShopperProductColorAsync(shopperProductColorId);

            return new ProductDetailViewModel()
            {
                Product = product,
                Category = category,
                ChildCategory = childCategory,
                Comments = comments,
                ProductGalleries = productGalleries,
                Shoppers = shoppers,
                Colors = colors,
                Shopper = shopper
            };
        }

        public async Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(string shopperProductColorId)
        {
            var product = await _productRepository.EditProductDetailShopperAsync(shopperProductColorId);
            int productId = await _productRepository.GetProductIdOfShopperProductColorIdAsync(shopperProductColorId);

            if (product == null || productId == 0)
                return null;

            var shoppers = _productRepository.GetProductShoppers(productId, product.SelectedColor);
            var colors = _colorRepository.GetProductColorsWithDetail(productId);


            product.Shoppers = shoppers;
            product.Colors = colors;

            return product;
        }

        public async Task<Tuple<string, string>> GetProductRedirectionByShortKeyAsync(string key)
            =>
                await _productRepository.GetProductRedirectionByShortKeyAsync(key);

        public async Task<Tuple<string, string>> GetBestSellerOfProductAsync(int productId) =>
            await _productRepository.GetBestSellerOfProductAsync(productId);

        public async Task<EditMobileProductViewModel> GetTypeMobileProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeMobileProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypePowerBankProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeMobileCoverProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeLaptopProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeTabletProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeSpeakerProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeWristWatchProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeSmartWatchProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }


        public async Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeMemoryCardProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

        public async Task<EditAuxViewModel> GetTypeAUXProductDataAsync(int productId)
        {
            var model = await _productRepository.GetTypeAUXProductDataForEditAsync(productId);

            model.StoreTitles = _shopperRepository.GetStoreTitles();
            model.Brands = _brandRepository.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            model.OfficialProducts = _brandRepository.GetBrandOfficialProducts(model.SelectedBrand);
            model.ChildCategories = _brandRepository.GetChildCategoriesOfBrand(model.SelectedBrand);

            return model;
        }

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

        public async Task<Tuple<IEnumerable<ProductViewModel>, int, int>> GetUserFavoriteProductsWithPagination(string userId, string sortBy = "news", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _productRepository.GetUserFavoriteProductsCountWithTypeAsync(userId);

            var products = _productRepository.GetUserFavoriteProductsWithPagination(userId, sortBy.FixedText(), skip, take);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);


            return new Tuple<IEnumerable<ProductViewModel>, int, int>(products, pageId, totalPages);
        }

        public async Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId)
        {
            return await _productRepository.GetFavoriteProductAsync(favoriteProductId);
        }

        public async Task<ResultTypes> DeleteFavoriteProductAsync(string userId, string shopperProductColorId)
        {
            try
            {
                var favoriteProduct = await _productRepository.GetFavoriteProductAsync(userId, shopperProductColorId);

                if (favoriteProduct == null)
                    return ResultTypes.Failed;

                _productRepository.RemoveFavoriteProduct(favoriteProduct);
                await _productRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
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

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductsDataChart()
        {
            var data = _productRepository.GetLastThirtyDayProductsDataChart();

            if (data == null)
                return null;

            var finalData = data.GroupBy(c => c.Date)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Key,
                    SellCount = c.Sum(g => g.SellCount),
                    ViewCount = 10
                }).ToList();

            if (!finalData.Any())
                return null;

            return finalData;
        }

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId)
        {
            var data = _productRepository.GetLastThirtyDayProductDataChart(productId);

            if (data == null)
                return null;

            var finalData = data.GroupBy(c => c.Date)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Key,
                    SellCount = c.Sum(g => g.SellCount),
                    ViewCount = 10
                });

            if (!finalData.Any())
                return null;

            return finalData;
        }

        public IEnumerable<Chipset> GetChipsets() => _productRepository.GetChipsets();

        public IEnumerable<Cpu> GetCpusOfChipset(string chipsetId) =>
            _productRepository.GetCpusOfChipset(chipsetId);

        public IEnumerable<Gpu> GetGpusOfChipset(string chipsetId) =>
            _productRepository.GetGpusOfChipset(chipsetId);

        public IEnumerable<CpuArch> GetCpuArches() => _productRepository.GetCpuArches();

        public IEnumerable<OperatingSystem> GetOperatingSystems() => _productRepository.GetOperatingSystems();

        public IEnumerable<OperatingSystemVersion> GetOperatingSystemVersionsOfOperatingSystem(string operatingSystemId) =>
            _productRepository.GetOperatingSystemVersionsOfOperatingSystem(operatingSystemId);
    }
}