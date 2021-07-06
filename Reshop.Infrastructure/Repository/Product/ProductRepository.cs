using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Product;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Infrastructure.Repository.Product
{
    public class ProductRepository : IProductRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public ProductRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion


        public IEnumerable<ProductViewModel> GetProductsWithPagination(string type, string sortBy, int skip, int take)
        {
            IQueryable<Domain.Entities.Product.Product> products = _context.Products
                .Include(c => c.ShopperProducts)
                .Skip(skip).Take(take);


            #region filter product

            if (type == "all")
            {
                products = sortBy switch
                {
                    "news" => products.OrderByDescending(c => c.CreateDate),
                    "expensive" => products.OrderByDescending(c => c.ShopperProducts.Max(c => c.Price)),
                    "cheap" => products.OrderBy(c => c.ShopperProducts.Min(c => c.Price)),
                    "mostsale" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.SaleCount).Sum()),
                    "mostviews" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.ViewCount).Sum()),
                    _ => products
                };
            }
            else
            {
                products = sortBy switch
                {
                    "news" => products.Where(c => c.ProductType == type).OrderByDescending(c => c.CreateDate),
                    "expensive" => products.Where(c => c.ProductType == type)
                        .OrderByDescending(c => c.ShopperProducts.Max(c => c.Price)),
                    "cheap" => products.Where(c => c.ProductType == type).OrderBy(c => c.ShopperProducts.Min(c => c.Price)),
                    "mostsale" => products.Where(c => c.ProductType == type).OrderByDescending(c => c.ShopperProducts.Select(c => c.SaleCount).Sum()),
                    "mostviews" => products.Where(c => c.ProductType == type).OrderByDescending(c => c.ShopperProducts.Select(c => c.ViewCount).Sum()),
                    _ => products
                };
            }

            #endregion


            return products.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductTitle = c.ProductTitle,
                BrandName = c.Brand.,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperUserId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperUserId,
            });
        }


        public IEnumerable<ProductViewModel> GetProductsOfCategoryWithPagination(int categoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<string> brands = null)
        {
            IQueryable<Domain.Entities.Product.Product> products = _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategory)
                .SelectMany(c => c.ProductToChildCategories)
                .Select(c => c.Product)
                .Skip(skip).Take(take);


            #region filter product


            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.CreateDate),
                "expensive" => products.OrderByDescending(c => c.ShopperProducts.Max(c => c.Price)),
                "cheap" => products.OrderBy(c => c.ShopperProducts.Min(c => c.Price)),
                "mostsale" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.SaleCount).Sum()),
                "mostviews" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.ViewCount).Sum()),
                _ => products
            };



            if (minPrice != 0)
            {
                products = products.Where(c => c.ShopperProducts.First().Price >= minPrice);
            }

            if (maxPrice != 0)
            {
                products = products.Where(c => c.ShopperProducts.First().Price <= maxPrice);
            }

            if (filter != null)
            {
                products = products.Where(c => c.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {


            }



            #endregion


            return products.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductTitle = c.ProductTitle,
                BrandName = c.Brand,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperUserId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperUserId,
            });
        }

        public IEnumerable<ProductViewModel> GetProductsOfChildCategoryWithPagination(int childCategoryId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<string> brands = null)
        {
            IQueryable<Domain.Entities.Product.Product> products = _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Product)
                .Skip(skip).Take(take);


            #region filter product

            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.CreateDate),
                "expensive" => products.OrderByDescending(c => c.ShopperProducts.Max(c => c.Price)),
                "cheap" => products.OrderBy(c => c.ShopperProducts.Min(c => c.Price)),
                "mostsale" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.SaleCount).Sum()),
                "mostviews" => products.OrderByDescending(c => c.ShopperProducts.Select(c => c.ViewCount).Sum()),
                _ => products
            };



            if (minPrice != 0)
            {
                products = products.Where(c => c.ShopperProducts.First().Price >= minPrice);
            }

            if (maxPrice != 0)
            {
                products = products.Where(c => c.ShopperProducts.First().Price <= maxPrice);
            }

            if (filter != null)
            {
                products = products.Where(c => c.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {


            }

            #endregion


            return products.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductTitle = c.ProductTitle,
                BrandName = c.Brand,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperUserId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperUserId,
            });
        }

        public async Task<string> GetProductFirstPictureName(int productId)
        {
            return await _context.ProductGalleries
                .Where(c => c.ProductId == productId).Select(c => c.ImageName).FirstOrDefaultAsync();
        }

        public IEnumerable<string> GetBrandsOfCategory(int categoryId) =>
            _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategory)
                .SelectMany(c => c.ProductToChildCategories)
                .Select(c => c.Product.Brand).Distinct();

        public IEnumerable<string> GetBrandsOfChildCategory(int childCategoryId) =>
            _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Product.Brand).Distinct();

        public async Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key)
            =>
                await _context.Products.SingleOrDefaultAsync(c => c.ShortKey == key);

        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId) =>
            await _context.Products.FindAsync(productId);

        public IEnumerable<ProductViewModel> GetShopperProductsWithPagination(string shopperUserId, string type, string sortBy, int skip, int take)
        {
            IQueryable<ShopperProduct> shopperProducts = _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId)
                .Skip(skip).Take(take);


            #region filter product

            if (type == "all")
            {
                shopperProducts = sortBy switch
                {
                    "news" => shopperProducts.OrderByDescending(c => c.Product.CreateDate),
                    "expensive" => shopperProducts.OrderByDescending(c => c.Price),
                    "cheap" => shopperProducts.OrderBy(c => c.Price),
                    "mostsale" => shopperProducts.OrderByDescending(c => c.SaleCount),
                    "mostviews" => shopperProducts.OrderByDescending(c => c.ViewCount),
                    _ => shopperProducts
                };
            }
            else
            {
                shopperProducts = sortBy switch
                {
                    "news" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.CreateDate),
                    "expensive" => shopperProducts.Where(c => c.Product.ProductType == type)
                        .OrderByDescending(c => c.Price),
                    "cheap" => shopperProducts.Where(c => c.Product.ProductType == type).OrderBy(c => c.Price),
                    "mostsale" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.SaleCount),
                    "mostviews" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.ViewCount),
                    _ => shopperProducts
                };
            }

            #endregion


            return shopperProducts.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductTitle = c.Product.ProductTitle,
                BrandName = c.Product.Brand,
                ProductPrice = c.Price,
                ShopperUserId = c.ShopperUserId
            });
        }

        public async Task<int> GetShopperProductsCountWithTypeAsync(string shopperUserId, string type)
        {
            if (type == "all")
            {
                return await _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                    .Select(c => c.Product).CountAsync();
            }

            return await _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                .Select(c => c.Product).Where(c => c.ProductType == type).CountAsync();
        }

        public IEnumerable<ProductViewModel> GetUnFinallyShopperProductRequestsWithPagination(string type, string sortBy, int skip, int take)
        {
            IQueryable<ShopperProduct> shopperProducts = _context.ShopperProducts
                .Where(c => !c.IsFinally)
                .Skip(skip).Take(take);

            #region filter product

            if (type == "all")
            {
                shopperProducts = sortBy switch
                {
                    "news" => shopperProducts.OrderByDescending(c => c.Product.CreateDate),
                    "expensive" => shopperProducts.OrderByDescending(c => c.Price),
                    "cheap" => shopperProducts.OrderBy(c => c.Price),
                    "mostsale" => shopperProducts.OrderByDescending(c => c.SaleCount),
                    "mostviews" => shopperProducts.OrderByDescending(c => c.ViewCount),
                    _ => shopperProducts
                };
            }
            else
            {
                shopperProducts = sortBy switch
                {
                    "news" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.CreateDate),
                    "expensive" => shopperProducts.Where(c => c.Product.ProductType == type)
                        .OrderByDescending(c => c.Price),
                    "cheap" => shopperProducts.Where(c => c.Product.ProductType == type).OrderBy(c => c.Price),
                    "mostsale" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.SaleCount),
                    "mostviews" => shopperProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.ViewCount),
                    _ => shopperProducts
                };
            }

            #endregion


            return shopperProducts.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductTitle = c.Product.ProductTitle,
                BrandName = c.Product.Brand,
                ProductPrice = c.Price,
                ShopperUserId = c.ShopperUserId
            });
        }

        public async Task<int> GetProductsCountWithTypeAsync(string type)
        {
            if (type == "all")
            {
                return await _context.Products.CountAsync();
            }

            return await _context.Products.Where(c => c.ProductType == type).CountAsync();

        }

        public async Task<int> GetUserFavoriteProductsCountWithTypeAsync(string userId, string type)
        {
            if (type == "all")
            {
                return await _context.FavoriteProducts.Where(c => c.UserId == userId).CountAsync();
            }

            return await _context.FavoriteProducts.Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .Where(c => c.Product.ProductType == type).CountAsync();
        }

        public async Task<int> GetCategoryProductsCountWithTypeAsync(int categoryId, string type = "")
        {
            if (type == "all")
            {
                return await _context.ChildCategoryToCategories
                    .Where(c => c.CategoryId == categoryId)
                    .Select(c => c.ChildCategory)
                    .SelectMany(c => c.ProductToChildCategories)
                    .Select(c => c.Product).CountAsync();
            }

            return await _context.ChildCategoryToCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ChildCategory)
                .SelectMany(c => c.ProductToChildCategories)
                .Select(c => c.Product)
                .Where(c => c.ProductType == type).CountAsync();
        }

        public async Task<int> GetChildCategoryProductsCountWithTypeAsync(int childCategoryId, string type = "")
        {
            if (string.IsNullOrEmpty(type))
            {
                return await _context.ProductToChildCategories
                    .Where(c => c.ChildCategoryId == childCategoryId)
                    .Select(c => c.Product).CountAsync();
            }
            return await _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Product)
                .Where(c => c.ProductType == type).CountAsync();
        }

        public async Task<Domain.Entities.Product.Product> GetProductWithTypeAsync(int productId, string type)
        {
            return type switch
            {
                "Mobile" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.MobileDetail)
                    .SingleOrDefaultAsync(),

                "MobileCover" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.MobileCoverDetail)
                    .SingleOrDefaultAsync(),

                "Laptop" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.LaptopDetail)
                    .SingleOrDefaultAsync(),

                "Speaker" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.SpeakerDetail)
                    .SingleOrDefaultAsync(),

                "FlashMemory" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.FlashMemoryDetail)
                    .SingleOrDefaultAsync(),

                "Handsfree" or "HeadPhone" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.HandsfreeAndHeadPhoneDetail)
                    .SingleOrDefaultAsync(),

                "Tablet" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.TabletDetail)
                    .SingleOrDefaultAsync(),

                "WristWatch" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.WristWatchDetail)
                    .SingleOrDefaultAsync(),

                "SmartWatch" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.MobileDetail)
                    .SingleOrDefaultAsync(),

                "PowerBank" => await _context.Products.Where(c => c.ProductId == productId)
                    .Include(c => c.PowerBankDetail)
                    .SingleOrDefaultAsync(),

                _ => await _context.Products.FindAsync(productId)
            };
        }

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperUserId, int productId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                .Include(c => c.Product).SingleOrDefaultAsync();

        public async Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId)
        {
            return await _context.MobileDetails.FindAsync(mobileDetailId);
        }

        public async Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId)
        {
            return await _context.LaptopDetails.FindAsync(laptopDetailId);
        }

        public async Task<MobileCoverDetail> GetMobileCoverDetailByIdAsync(int mobileCoverId)
            =>
                await _context.MobileCoverDetails.FindAsync(mobileCoverId);

        public async Task<TabletDetail> GetTabletDetailByIdAsync(int tabletDetailId)
            =>
                await _context.TabletDetails.FindAsync(tabletDetailId);

        public async Task<FlashMemoryDetail> GetFlashMemoryDetailByIdAsync(int flashMemoryId)
            =>
                await _context.FlashMemoryDetails.FindAsync(flashMemoryId);

        public async Task<SpeakerDetail> GetSpeakerDetailByIdAsync(int speakerDetailId)
            =>
                await _context.SpeakerDetails.FindAsync(speakerDetailId);

        public async Task<SmartWatchDetail> GetSmartWatchDetailByIdAsync(int smartWatchDetailId)
            =>
                await _context.SmartWatchDetails.FindAsync(smartWatchDetailId);

        public async Task<WristWatchDetail> GetWristWatchDetailByIdAsync(int wristWatchDetailId)
            =>
                await _context.WristWatchDetails.FindAsync(wristWatchDetailId);

        public async Task<MemoryCardDetail> GetMemoryCardDetailByIdAsync(int memoryCardDetailId)
            =>
                await _context.MemoryCardDetails.FindAsync(memoryCardDetailId);

        public async Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeOrHeadPhoneDetailId)
            =>
                await _context.HandsfreeAndHeadPhoneDetails.FindAsync(handsfreeOrHeadPhoneDetailId);

        public IAsyncEnumerable<ProductGallery> GetProductImages(int productId)
        {
            return _context.ProductGalleries.Where(c => c.ProductId == productId) as IAsyncEnumerable<ProductGallery>;
        }

        public void RemoveMobileCoverDetail(MobileCoverDetail mobileCoverDetail)
            =>
                _context.Remove(mobileCoverDetail);

        public IEnumerable<ChildCategory> GetProductChildCategories(int productId)
        {
            return _context.ProductToChildCategories
                .Where(c => c.ProductId == productId)
                .Select(c => c.ChildCategory);
        }

        public IEnumerable<Comment> GetProductComments(int productId)
        {
            return _context.Comments.Where(c => c.ProductId == productId)
                .Include(c => c.User);
        }

        public IEnumerable<Question> GetProductQuestions(int productId)
        {
            return _context.Questions.Where(c => c.ProductId == productId)
                .Include(c => c.QuestionAnswers);
        }

        public IEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string type, string sortBy, int skip = 0, int take = 24)
        {
            IQueryable<FavoriteProduct> favoriteProducts = _context.FavoriteProducts
                .Where(c => c.UserId == userId)
                .Skip(skip).Take(take)
                .Include(c => c.Product);






            #region filter product

            if (type == "all")
            {
                favoriteProducts = sortBy switch
                {
                    "news" => favoriteProducts.OrderByDescending(c => c.Product.CreateDate),
                    "expensive" => favoriteProducts.OrderByDescending(c => c.Product.ShopperProducts.Max(c => c.Price)),
                    "cheap" => favoriteProducts.OrderBy(c => c.Product.ShopperProducts.Min(c => c.Price)),
                    "mostsale" => favoriteProducts.OrderByDescending(c => c.Product.ShopperProducts.Select(c => c.SaleCount).Sum()),
                    "mostviews" => favoriteProducts.OrderByDescending(c => c.Product.ShopperProducts.Select(c => c.ViewCount).Sum()),
                    _ => favoriteProducts
                };
            }
            else
            {
                favoriteProducts = sortBy switch
                {
                    "news" => favoriteProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.Product.CreateDate),
                    "expensive" => favoriteProducts.Where(c => c.Product.ProductType == type)
                        .OrderByDescending(c => c.Product.ShopperProducts.Max(c => c.Price)),
                    "cheap" => favoriteProducts.Where(c => c.Product.ProductType == type).OrderBy(c => c.Product.ShopperProducts.Min(c => c.Price)),
                    "mostsale" => favoriteProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.Product.ShopperProducts.Select(c => c.SaleCount).Sum()),
                    "mostviews" => favoriteProducts.Where(c => c.Product.ProductType == type).OrderByDescending(c => c.Product.ShopperProducts.Select(c => c.ViewCount).Sum()),
                    _ => favoriteProducts
                };
            }

            #endregion

            return favoriteProducts.Select(c => new ProductViewModel()
            {
                ProductId = c.Product.ProductId,
                ProductTitle = c.Product.ProductTitle,
                BrandName = c.Product.Brand,
                ProductPrice = _context.ShopperProducts.SingleOrDefault(s => s.ShopperUserId == c.ShopperUserId && s.ProductId == c.ProductId).Price,
                ShopperUserId = c.ShopperUserId
            });
        }

        public async Task AddToFavoriteProductAsync(FavoriteProduct favoriteProduct)
        {
            await _context.FavoriteProducts.AddAsync(favoriteProduct);
        }

        public void RemoveFavoriteProduct(FavoriteProduct favoriteProduct)
        {
            _context.FavoriteProducts.Remove(favoriteProduct);
        }

        public void UpdateFavoriteProduct(FavoriteProduct favoriteProduct) =>
            _context.FavoriteProducts.Update(favoriteProduct);

        public async Task<bool> IsFavoriteProductExistAsync(string favoriteProductId)
        {
            return await _context.FavoriteProducts.AnyAsync(c => c.FavoriteProductId == favoriteProductId);
        }

        public async Task<bool> IsFavoriteProductExistAsync(string userId, int productId) =>
            await _context.FavoriteProducts.AnyAsync(c => c.UserId == userId && c.ProductId == productId);

        public async Task<FavoriteProduct> GetFavoriteProductAsync(string favoriteProductId)
        {
            return await _context.FavoriteProducts.FindAsync(favoriteProductId);
        }

        public async Task<FavoriteProduct> GetFavoriteProductAsync(string userId, int productId) =>
            await _context.FavoriteProducts.SingleOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        public IAsyncEnumerable<string> GetProductsNameByFilter(string productName)
        {
            return _context.Products.Where(c => c.ProductTitle.Contains(productName))
                .Select(c => c.ProductTitle) as IAsyncEnumerable<string>;
        }

        public IAsyncEnumerable<ProductViewModel> GetProductsByFilter(string productName)
        {
            return _context.Products.Where(c => c.ProductTitle.Contains(productName))
                .Include(c => c.ShopperProducts)
                .Select(c => new ProductViewModel()
                {
                    ProductId = c.ProductId,
                    ProductPrice = c.ShopperProducts.First().Price,
                    ProductTitle = c.ProductTitle,
                    BrandName = c.Brand
                }) as IAsyncEnumerable<ProductViewModel>;
        }

        public async Task AddUserProductViewAsync(UserProductView userProductView) =>
            await _context.UserProductsView.AddAsync(userProductView);

        public async Task<bool> IsUserProductViewExistAsync(int productId, string userIP) =>
            await _context.UserProductsView.AnyAsync(c => c.ProductId == productId && c.UserIPAddress == userIP);

        public IEnumerable<Domain.Entities.Product.Product> GetTypeMobileProducts()
        {
            return _context.Products
                .Include(c => c.MobileDetail);
        }

        public IEnumerable<Domain.Entities.Product.Product> GetTypeLaptopProducts()
        {
            return _context.Products
                .Include(c => c.LaptopDetail);
        }

        public async Task<ProductGallery> GetProductGalleryByIdAsync(string productGalleryId)
            =>
                await _context.ProductGalleries.FindAsync(productGalleryId);

        public async Task<int> GetProductGalleriesCountByProductIdAsync(int productId)
            =>
                await _context.ProductGalleries.Where(c => c.ProductId == productId).CountAsync();

        public async Task<bool> IsProductExistAsync(int productId)
        {
            return await _context.Products.AnyAsync(c => c.ProductId == productId);
        }

        public async Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId, string shopperUserId)
        {
            return await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                .Select(c => new AddOrEditMobileProductViewModel()
                {
                    ProductId = c.Product.ProductId,
                    ProductTitle = c.Product.ProductTitle,
                    Description = c.Product.Description,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock,
                    BrandProduct = c.Product.BrandProduct,
                    ProductBrand = c.Product.Brand,
                    InternalMemory = c.Product.MobileDetail.InternalMemory,
                    CommunicationNetworks = c.Product.MobileDetail.CommunicationNetworks,
                    BackCameras = c.Product.MobileDetail.BackCameras,
                    OperatingSystem = c.Product.MobileDetail.OperatingSystem,
                    SIMCardDescription = c.Product.MobileDetail.SIMCardDescription,
                    RAMValue = c.Product.MobileDetail.RAMValue,
                    PhotoResolution = c.Product.MobileDetail.PhotoResolution,
                    OperatingSystemVersion = c.Product.MobileDetail.OperatingSystemVersion,
                    DisplayTechnology = c.Product.MobileDetail.DisplayTechnology,
                    Features = c.Product.MobileDetail.Features,
                    Size = c.Product.MobileDetail.Size,
                    QuantitySIMCard = c.Product.MobileDetail.QuantitySIMCard,
                }).SingleOrDefaultAsync();
        }

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId, string shopperUserId) =>
             await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                 .Select(c => new AddOrEditLaptopProductViewModel()
                 {
                     ProductId = c.Product.ProductId,
                     ProductTitle = c.Product.ProductTitle,
                     Description = c.Product.Description,
                     Price = c.Price,
                     QuantityInStock = c.QuantityInStock,
                     BrandProduct = c.Product.BrandProduct,
                     ProductBrand = c.Product.Brand,
                     RAMCapacity = c.Product.LaptopDetail.RAMCapacity,
                     InternalMemory = c.Product.LaptopDetail.InternalMemory,
                     GPUManufacturer = c.Product.LaptopDetail.GPUManufacturer,
                     Size = c.Product.LaptopDetail.Size,
                     Category = c.Product.LaptopDetail.Category,
                     ProcessorSeries = c.Product.LaptopDetail.ProcessorSeries,
                     RAMType = c.Product.LaptopDetail.RAMType,
                     ScreenAccuracy = c.Product.LaptopDetail.ScreenAccuracy,
                     IsMatteScreen = c.Product.LaptopDetail.IsMatteScreen,
                     IsTouchScreen = c.Product.LaptopDetail.IsTouchScreen,
                     OperatingSystem = c.Product.LaptopDetail.OperatingSystem,
                     IsHDMIPort = c.Product.LaptopDetail.IsHDMIPort,
                 }).SingleOrDefaultAsync();


        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
            .Select(c => new AddOrEditMobileCoverViewModel()
            {
                ProductId = c.Product.ProductId,
                ProductTitle = c.Product.ProductTitle,
                Description = c.Product.Description,
                Price = c.Price,
                QuantityInStock = c.QuantityInStock,
                BrandProduct = c.Product.BrandProduct,
                ProductBrand = c.Product.Brand,
                SuitablePhones = c.Product.MobileCoverDetail.SuitablePhones,
                Gender = c.Product.MobileCoverDetail.Gender,
                Structure = c.Product.MobileCoverDetail.Structure,
                CoverLevel = c.Product.MobileCoverDetail.CoverLevel,
                Features = c.Product.MobileCoverDetail.Features
            }).SingleOrDefaultAsync();

        public async Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditFlashMemoryViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        Connector = c.Product.FlashMemoryDetail.Connector,
                        Capacity = c.Product.FlashMemoryDetail.Capacity,
                        IsImpactResistance = c.Product.FlashMemoryDetail.IsImpactResistance
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditHandsfreeAndHeadPhoneViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        ConnectionType = c.Product.HandsfreeAndHeadPhoneDetail.ConnectionType,
                        PhoneType = c.Product.HandsfreeAndHeadPhoneDetail.PhoneType,
                        WorkSuggestion = c.Product.HandsfreeAndHeadPhoneDetail.WorkSuggestion,
                        Connector = c.Product.HandsfreeAndHeadPhoneDetail.Connector,
                        IsSupportBattery = c.Product.HandsfreeAndHeadPhoneDetail.IsSupportBattery,
                        Features = c.Product.HandsfreeAndHeadPhoneDetail.Features
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditTabletViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        InternalMemory = c.Product.TabletDetail.InternalMemory,
                        RAMValue = c.Product.TabletDetail.RAMValue,
                        IsTalkAbility = c.Product.TabletDetail.IsTalkAbility,
                        Size = c.Product.TabletDetail.Size,
                        CommunicationNetworks = c.Product.TabletDetail.CommunicationNetworks,
                        Features = c.Product.TabletDetail.Features,
                        IsSIMCardSupporter = c.Product.TabletDetail.IsSIMCardSupporter,
                        QuantitySIMCard = c.Product.TabletDetail.QuantitySIMCard,
                        OperatingSystemVersion = c.Product.TabletDetail.OperatingSystemVersion,
                        CommunicationTechnologies = c.Product.TabletDetail.CommunicationTechnologies,
                        CommunicationPorts = c.Product.TabletDetail.CommunicationPorts
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditSpeakerViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        ConnectionType = c.Product.SpeakerDetail.ConnectionType,
                        Connector = c.Product.SpeakerDetail.Connector,
                        BluetoothVersion = c.Product.SpeakerDetail.BluetoothVersion,
                        IsMemoryCardInput = c.Product.SpeakerDetail.IsMemoryCardInput,
                        IsSupportBattery = c.Product.SpeakerDetail.IsSupportBattery,
                        IsSupportMicrophone = c.Product.SpeakerDetail.IsSupportMicrophone,
                        IsSupportUSBPort = c.Product.SpeakerDetail.IsSupportUSBPort,
                        IsSupportRadio = c.Product.SpeakerDetail.IsSupportRadio
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEdirWristWatchViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        IsSupportGPS = c.Product.WristWatchDetail.IsSupportGPS,
                        IsTouchScreen = c.Product.WristWatchDetail.IsTouchScreen,
                        WatchForm = c.Product.WristWatchDetail.WatchForm
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditSmartWatchViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        IsSuitableForMen = c.Product.SmartWatchDetail.IsSuitableForMen,
                        IsSuitableForWomen = c.Product.SmartWatchDetail.IsSuitableForWomen,
                        IsScreenColorful = c.Product.SmartWatchDetail.IsScreenColorful,
                        IsSIMCardSupporter = c.Product.SmartWatchDetail.IsSIMCardSupporter,
                        IsTouchScreen = c.Product.SmartWatchDetail.IsTouchScreen,
                        IsSupportSIMCardRegister = c.Product.SmartWatchDetail.IsSupportSIMCardRegister,
                        WorkSuggestion = c.Product.SmartWatchDetail.WorkSuggestion,
                        IsSupportGPS = c.Product.SmartWatchDetail.IsSupportGPS,
                        WatchForm = c.Product.SmartWatchDetail.WatchForm,
                        BodyMaterial = c.Product.SmartWatchDetail.BodyMaterial,
                        Connections = c.Product.SmartWatchDetail.Connections,
                        Sensors = c.Product.SmartWatchDetail.Sensors,
                        IsDirectTalkable = c.Product.SmartWatchDetail.IsDirectTalkable,
                        IsTalkableWithBluetooth = c.Product.SmartWatchDetail.IsTalkableWithBluetooth
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId, string shopperUserId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperUserId == shopperUserId && c.ProductId == productId)
                    .Select(c => new AddOrEditMemoryCardViewModel()
                    {
                        ProductId = c.Product.ProductId,
                        ProductTitle = c.Product.ProductTitle,
                        Description = c.Product.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.Product.BrandProduct,
                        ProductBrand = c.Product.Brand,
                        Capacity = c.Product.MemoryCardDetail.Capacity,
                        Size = c.Product.MemoryCardDetail.Size,
                        SpeedStandard = c.Product.MemoryCardDetail.SpeedStandard,
                        ResistsAgainst = c.Product.MemoryCardDetail.ResistsAgainst
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<string> GetProductTypeAsync(int productId)
        {
            return await _context.Products.Where(c => c.ProductId == productId)
                .Select(c => c.ProductType).SingleOrDefaultAsync();
        }

        public async Task AddProductAsync(Domain.Entities.Product.Product product)
        {
            await _context.AddAsync(product);
        }

        public async Task AddMobileDetailAsync(MobileDetail mobileDetail)
        {
            await _context.MobileDetails.AddAsync(mobileDetail);
        }

        public async Task AddLaptopDetailAsync(LaptopDetail laptopDetail)
        {
            await _context.LaptopDetails.AddAsync(laptopDetail);
        }

        public async Task AddProductGalley(ProductGallery productGallery)
            =>
                await _context.ProductGalleries.AddAsync(productGallery);

        public async Task AddMobileCoverDetailAsync(MobileCoverDetail mobileCoverDetail)
        {
            await _context.MobileCoverDetails.AddAsync(mobileCoverDetail);
        }

        public async Task AddTabletDetailAsync(TabletDetail tabletDetail)
            =>
                await _context.TabletDetails.AddAsync(tabletDetail);

        public async Task AddHandsfreeAndHeadPhoneDetailAsync(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail)
            =>
                await _context.HandsfreeAndHeadPhoneDetails.AddAsync(handsfreeAndHeadPhoneDetail);

        public async Task AddFlashMemoryDetailAsync(FlashMemoryDetail flashMemoryDetail)
            =>
                await _context.FlashMemoryDetails.AddAsync(flashMemoryDetail);

        public async Task AddSpeakerDetailAsync(SpeakerDetail speakerDetail)
            =>
                await _context.SpeakerDetails.AddAsync(speakerDetail);

        public async Task AddWristWatchDetailAsync(WristWatchDetail wristWatchDetail)
            =>
                await _context.WristWatchDetails.AddAsync(wristWatchDetail);

        public async Task AddSmartWatchAsync(SmartWatchDetail smartWatchDetail)
            =>
                await _context.SmartWatchDetails.AddAsync(smartWatchDetail);

        public async Task AddMemoryCardDetailAsync(MemoryCardDetail memoryCardDetail)
            =>
                await _context.MemoryCardDetails.AddAsync(memoryCardDetail);

        public void UpdateProduct(Domain.Entities.Product.Product product)
        {
            _context.Products.Update(product);
        }

        public void UpdateMobileDetail(MobileDetail mobileDetail)
        {
            _context.MobileDetails.Update(mobileDetail);
        }

        public void UpdateLaptopDetail(LaptopDetail laptopDetail)
        {
            _context.LaptopDetails.Update(laptopDetail);
        }

        public void UpdateMobileCoverDetail(MobileCoverDetail mobileCoverDetail)
            =>
                _context.MobileCoverDetails.Update(mobileCoverDetail);

        public void UpdateTabletDetail(TabletDetail tabletDetail)
            =>
                _context.TabletDetails.Update(tabletDetail);

        public void UpdateHandsfreeAndHeadPhoneDetail(HandsfreeAndHeadPhoneDetail handsfreeAndHeadPhoneDetail)
            =>
                _context.HandsfreeAndHeadPhoneDetails.Update(handsfreeAndHeadPhoneDetail);

        public void UpdateFlashMemoryDetail(FlashMemoryDetail flashMemoryDetail)
            =>
                _context.FlashMemoryDetails.Update(flashMemoryDetail);

        public void UpdateSpeakerDetail(SpeakerDetail speakerDetail)
            =>
                _context.SpeakerDetails.Update(speakerDetail);

        public void UpdateWristWatchDetail(WristWatchDetail wristWatchDetail)
            =>
                _context.WristWatchDetails.Update(wristWatchDetail);

        public void UpdateSmartWatch(SmartWatchDetail smartWatchDetail)
            =>
                _context.SmartWatchDetails.Update(smartWatchDetail);

        public void UpdateMemoryCardDetail(MemoryCardDetail memoryCardDetail)
            =>
                _context.MemoryCardDetails.Update(memoryCardDetail);

        public async Task<bool> IsProductExistByShortKeyAsync(string shortKey)
        {
            return await _context.Products.AnyAsync(c => c.ShortKey == shortKey);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void RemoveProduct(Domain.Entities.Product.Product product)
        {
            _context.Products.Remove(product);
        }

        public void RemoveMobileDetail(MobileDetail mobileDetail)
        {
            _context.MobileDetails.Remove(mobileDetail);
        }

        public void RemoveLaptopDetail(LaptopDetail laptopDetail)
        {
            _context.LaptopDetails.Remove(laptopDetail);
        }

        public void RemoveRangeProducts(List<Domain.Entities.Product.Product> products)
        {
            _context.Products.RemoveRange(products);
        }

        public void RemoveRangeMobileDetails(List<MobileDetail> mobileDetails)
        {
            _context.MobileDetails.RemoveRange(mobileDetails);
        }

        public void RemoveRangeLaptopDetails(List<LaptopDetail> laptopDetails)
        {
            _context.LaptopDetails.RemoveRange(laptopDetails);
        }
    }
}