using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Product;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                BrandName = c.Brand.BrandName,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperId,
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
                BrandName = c.Brand.BrandName,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperId,
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
                BrandName = c.Brand.BrandName,
                ProductPrice = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().Price,
                ShopperId = c.ShopperProducts.OrderByDescending(s => s.SaleCount).First().ShopperId,
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
                .Select(c => c.Product.Brand.BrandName).Distinct();

        public IEnumerable<string> GetBrandsOfChildCategory(int childCategoryId) =>
            _context.ProductToChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .Select(c => c.Product.Brand.BrandName).Distinct();

        public async Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key)
            =>
                await _context.Products.SingleOrDefaultAsync(c => c.ShortKey == key);

        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId) =>
            await _context.Products.FindAsync(productId);

        public IEnumerable<ProductViewModel> GetShopperProductsWithPagination(string shopperId, string type, string sortBy, int skip, int take)
        {
            IQueryable<ShopperProduct> shopperProducts = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId)
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
                BrandName = c.Product.Brand.BrandName,
                ProductPrice = c.Price,
                ShopperId = c.ShopperId
            });
        }

        public async Task<int> GetShopperProductsCountWithTypeAsync(string shopperId, string type)
        {
            if (type == "all")
            {
                return await _context.ShopperProducts.Where(c => c.ShopperId == shopperId)
                    .Select(c => c.Product).CountAsync();
            }

            return await _context.ShopperProducts.Where(c => c.ShopperId == shopperId)
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
                BrandName = c.Product.Brand.BrandName,
                ProductPrice = c.Price,
                ShopperId = c.ShopperId
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

        public async Task<ShopperProduct> GetProductWithTypeAsync(int productId, string type, string shopperId = "")
        {
            if (string.IsNullOrEmpty(shopperId))
            {
                return type switch
                {
                    "mobile" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.MobileDetail).FirstAsync(),

                    "mobilecover" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.MobileCoverDetail).FirstAsync(),

                    "laptop" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.LaptopDetailId).FirstAsync(),

                    "speaker" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.SpeakerDetail).FirstAsync(),

                    "flashmemory" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.FlashMemoryDetail).FirstAsync(),

                    "handsfree" or "headphone" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.HandsfreeAndHeadPhoneDetail).FirstAsync(),

                    "tablet" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                    .Include(c => c.Product)
                    .ThenInclude(c => c.TabletDetail).FirstAsync(),

                    "wristwatch" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.WristWatchDetail).FirstAsync(),

                    "smartwatch" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.SmartWatchDetail).FirstAsync(),

                    "powerbank" => await _context.ShopperProducts.Where(c => c.ProductId == productId).OrderByDescending(c => c.SaleCount)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.PowerBankDetail).FirstAsync(),

                    _ => null
                };
            }
            else
            {
                return type switch
                {
                    "mobile" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.MobileDetail).FirstAsync(),

                    "mobilecover" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.MobileCoverDetail).FirstAsync(),

                    "laptop" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.LaptopDetailId).FirstAsync(),

                    "speaker" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.SpeakerDetail).FirstAsync(),

                    "flashmemory" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.FlashMemoryDetail).FirstAsync(),

                    "handsfree" or "headphone" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.HandsfreeAndHeadPhoneDetail).FirstAsync(),

                    "tablet" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                    .Include(c => c.Product)
                    .ThenInclude(c => c.TabletDetail).FirstAsync(),

                    "wristwatch" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.WristWatchDetail).FirstAsync(),

                    "smartwatch" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.SmartWatchDetail).FirstAsync(),

                    "powerbank" => await _context.ShopperProducts.Where(c => c.ProductId == productId && c.ShopperId == shopperId)
                        .Include(c => c.Product)
                        .ThenInclude(c => c.PowerBankDetail).FirstAsync(),

                    _ => null
                };
            }
        }

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperId, int productId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.ProductId == productId)
                .Include(c => c.Product).SingleOrDefaultAsync();

        public async Task<MobileDetail> GetMobileDetailByIdAsync(int mobileDetailId)
        {
            return await _context.MobileDetails.FindAsync(mobileDetailId);
        }

        public async Task<LaptopDetail> GetLaptopDetailByIdAsync(int laptopDetailId)
        {
            return await _context.LaptopDetails.FindAsync(laptopDetailId);
        }

        public async Task<PowerBankDetail> GetPowerBankDetailByIdAsync(int powerBankId)
        {
            return await _context.PowerBankDetails.FindAsync(powerBankId);
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

        public async Task<AUXDetail> GetAUXByIdAsync(int auxId)
            =>
                await _context.AuxDetails.FindAsync(auxId);

        public async Task<HandsfreeAndHeadPhoneDetail> GetHandsfreeAndHeadPhoneDetailByIdAsync(int handsfreeOrHeadPhoneDetailId)
            =>
                await _context.HandsfreeAndHeadPhoneDetails.FindAsync(handsfreeOrHeadPhoneDetailId);

        public IEnumerable<ProductGallery> GetProductImages(int productId)
        {
            return _context.ProductGalleries.Where(c => c.ProductId == productId);
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
                BrandName = c.Product.Brand.BrandName,
                ProductPrice = _context.ShopperProducts.SingleOrDefault(s => s.ShopperId == c.ShopperId && s.ProductId == c.ProductId).Price,
                ShopperId = c.ShopperId
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
                    BrandName = c.Brand.BrandName
                }) as IAsyncEnumerable<ProductViewModel>;
        }

       

    

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

        public async Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId)
        {
            return await _context.Products
                .Where(c => c.ProductId == productId)
                .Select(c => new AddOrEditMobileProductViewModel()
                {
                    ProductId = c.ProductId,
                    ProductTitle = c.ProductTitle,
                    Description = c.Description,
                    Brand = c.BrandId,
                    OfficialBrandProductId = c.OfficialBrandProductId.Value,
                    IsActive = c.IsActive,

                    //img
                    SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                    SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                    SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                    SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                    SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                    SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                    // detail
                    Lenght = c.MobileDetail.Lenght,
                    Width = c.MobileDetail.Width,
                    Height = c.MobileDetail.Height,
                    Weight = c.MobileDetail.Weight,
                    SimCardQuantity = c.MobileDetail.SimCardQuantity,
                    SimCardInpute = c.MobileDetail.SimCardInpute,
                    SeparateSlotMemoryCard = c.MobileDetail.SeparateSlotMemoryCard,
                    Announced = c.MobileDetail.Announced,
                    ChipsetName = c.MobileDetail.ChipsetName,
                    Cpu = c.MobileDetail.Cpu,
                    CpuAndFrequency = c.MobileDetail.CpuAndFrequency,
                    CpuArch = c.MobileDetail.CpuArch,
                    Gpu = c.MobileDetail.Gpu,
                    InternalStorage = c.MobileDetail.InternalStorage,
                    Ram = c.MobileDetail.Ram,
                    SdCard = c.MobileDetail.SdCard,
                    SdCardStandard = c.MobileDetail.SdCardStandard,
                    ColorDisplay = c.MobileDetail.ColorDisplay,
                    TouchDisplay = c.MobileDetail.TouchDisplay,
                    DisplayTechnology = c.MobileDetail.DisplayTechnology,
                    DisplaySize = c.MobileDetail.DisplaySize,
                    Resolution = c.MobileDetail.Resolution,
                    PixelDensity = c.MobileDetail.PixelDensity,
                    ScreenToBodyRatio = c.MobileDetail.ScreenToBodyRatio,
                    ImageRatio = c.MobileDetail.ImageRatio,
                    DisplayProtection = c.MobileDetail.DisplayProtection,
                    MoreInformation = c.MobileDetail.MoreInformation,
                    ConnectionsNetwork = c.MobileDetail.ConnectionsNetwork,
                    GsmNetwork = c.MobileDetail.GsmNetwork,
                    HspaNetwork = c.MobileDetail.HspaNetwork,
                    LteNetwork = c.MobileDetail.LteNetwork,
                    FiveGNetwork = c.MobileDetail.FiveGNetwork,
                    CommunicationTechnology = c.MobileDetail.CommunicationTechnology,
                    WiFi = c.MobileDetail.WiFi,
                    Radio = c.MobileDetail.Radio,
                    Bluetooth = c.MobileDetail.Bluetooth,
                    GpsInformation = c.MobileDetail.GpsInformation,
                    ConnectionPort = c.MobileDetail.ConnectionPort,
                    CameraQuantity = c.MobileDetail.CameraQuantity,
                    PhotoResolutation = c.MobileDetail.PhotoResolutation,
                    SelfiCameraPhoto = c.MobileDetail.SelfiCameraPhoto,
                    CameraCapabilities = c.MobileDetail.CameraCapabilities,
                    SelfiCameraCapabilities = c.MobileDetail.SelfiCameraCapabilities,
                    Filming = c.MobileDetail.Filming,
                    Speakers = c.MobileDetail.Speakers,
                    OutputAudio = c.MobileDetail.OutputAudio,
                    AudioInformation = c.MobileDetail.AudioInformation,
                    OS = c.MobileDetail.OS,
                    OsVersion = c.MobileDetail.OsVersion,
                    UiVersion = c.MobileDetail.UiVersion,
                    MoreInformationSoftWare = c.MobileDetail.MoreInformationSoftWare,
                    BatteryMaterial = c.MobileDetail.BatteryMaterial,
                    BatteryCapacity = c.MobileDetail.BatteryCapacity,
                    Removable‌Battery = c.MobileDetail.Removable‌Battery,
                    Sensors = c.MobileDetail.Sensors,
                    ItemsInBox = c.MobileDetail.ItemsInBox
                }).SingleOrDefaultAsync();
        }

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId) =>
             await _context.Products
                .Where(c => c.ProductId == productId)
                 .Select(c => new AddOrEditLaptopProductViewModel()
                 {
                     ProductId = c.ProductId,
                     ProductTitle = c.ProductTitle,
                     Description = c.Description,
                     Brand = c.BrandId,
                     OfficialBrandProductId = c.OfficialBrandProductId.Value,
                     IsActive = c.IsActive,

                     //img
                     SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                     SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                     SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                     SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                     SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                     SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                     // detail
                     Length = c.LaptopDetail.Length,
                     Width = c.LaptopDetail.Width,
                     Height = c.LaptopDetail.Height,
                     Weight = c.LaptopDetail.Weight,
                     CpuCompany = c.LaptopDetail.CpuCompany,
                     CpuSeries = c.LaptopDetail.CpuSeries,
                     CpuModel = c.LaptopDetail.CpuModel,
                     CpuFerequancy = c.LaptopDetail.CpuFerequancy,
                     CpuCache = c.LaptopDetail.CpuCache,
                     RamStorage = c.LaptopDetail.RamStorage,
                     RamStorageTeachnology = c.LaptopDetail.RamStorageTeachnology,
                     Storage = c.LaptopDetail.Storage,
                     StorageTeachnology = c.LaptopDetail.StorageTeachnology,
                     StorageInformation = c.LaptopDetail.StorageInformation,
                     GpuCompany = c.LaptopDetail.GpuCompany,
                     GpuModel = c.LaptopDetail.GpuModel,
                     GpuRam = c.LaptopDetail.GpuRam,
                     DisplaySize = c.LaptopDetail.DisplaySize,
                     DisplayTeachnology = c.LaptopDetail.DisplayTeachnology,
                     DisplayResolutation = c.LaptopDetail.DisplayResolutation,
                     RefreshDisplay = c.LaptopDetail.RefreshDisplay,
                     BlurDisplay = c.LaptopDetail.BlurDisplay,
                     TouchDisplay = c.LaptopDetail.TouchDisplay,
                     DiskDrive = c.LaptopDetail.DiskDrive,
                     FingerTouch = c.LaptopDetail.FingerTouch,
                     Webcam = c.LaptopDetail.Webcam,
                     BacklightKey = c.LaptopDetail.BacklightKey,
                     TouchPadInformation = c.LaptopDetail.TouchPadInformation,
                     ModemInformation = c.LaptopDetail.ModemInformation,
                     Wifi = c.LaptopDetail.Wifi,
                     Bluetooth = c.LaptopDetail.Bluetooth,
                     VgaPort = c.LaptopDetail.VgaPort,
                     HtmiPort = c.LaptopDetail.HtmiPort,
                     DisplayPort = c.LaptopDetail.DisplayPort,
                     LanPort = c.LaptopDetail.LanPort,
                     UsbCPort = c.LaptopDetail.UsbCPort,
                     Usb3Port = c.LaptopDetail.Usb3Port,
                     UsbCQuantity = c.LaptopDetail.UsbCQuantity,
                     UsbQuantity = c.LaptopDetail.UsbQuantity,
                     Usb3Quantity = c.LaptopDetail.Usb3Quantity,
                     BatteryMaterial = c.LaptopDetail.BatteryMaterial,
                     BatteryCharging = c.LaptopDetail.BatteryCharging,
                     BatteryInformation = c.LaptopDetail.BatteryInformation,
                     Os = c.LaptopDetail.Os,
                     Classification = c.LaptopDetail.Classification,
                 }).SingleOrDefaultAsync();

        public async Task<AddOrEditPowerBankViewModel> GetTypePowerBankProductDataForEditAsync(int productId) =>
             await _context.Products
                .Where(c => c.ProductId == productId)
                 .Select(c => new AddOrEditPowerBankViewModel()
                 {
                     ProductId = c.ProductId,
                     ProductTitle = c.ProductTitle,
                     Description = c.Description,
                     Brand = c.BrandId,
                     OfficialBrandProductId = c.OfficialBrandProductId.Value,
                     IsActive = c.IsActive,

                     //img
                     SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                     SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                     SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                     SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                     SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                     SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                     // detail
                     Length = c.PowerBankDetail.Length,
                     Width = c.PowerBankDetail.Width,
                     Height = c.PowerBankDetail.Height,
                     Weight = c.PowerBankDetail.Weight,
                     CapacityRange = c.PowerBankDetail.CapacityRange,
                     InputVoltage = c.PowerBankDetail.InputVoltage,
                     OutputVoltage = c.PowerBankDetail.OutputVoltage,
                     InputCurrentIntensity = c.PowerBankDetail.InputCurrentIntensity,
                     OutputCurrentIntensity = c.PowerBankDetail.OutputCurrentIntensity,
                     OutputPortsCount = c.PowerBankDetail.OutputPortsCount,
                     IsSupportOfQCTechnology = c.PowerBankDetail.IsSupportOfQCTechnology,
                     IsSupportOfPDTechnology = c.PowerBankDetail.IsSupportOfPDTechnology,
                     BodyMaterial = c.PowerBankDetail.BodyMaterial,
                     DisplayCharge = c.PowerBankDetail.DisplayCharge,
                     Features = c.PowerBankDetail.Features,
                 }).SingleOrDefaultAsync();

        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditMobileCoverViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        SuitablePhones = c.MobileCoverDetail.SuitablePhones,
                        Gender = c.MobileCoverDetail.Gender,
                        Structure = c.MobileCoverDetail.Structure,
                        CoverLevel = c.MobileCoverDetail.CoverLevel,
                        Features = c.MobileCoverDetail.Features,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditFlashMemoryViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        Length = c.FlashMemoryDetail.Length,
                        Width = c.FlashMemoryDetail.Width,
                        Height = c.FlashMemoryDetail.Height,
                        BodyMaterial = c.FlashMemoryDetail.BodyMaterial,
                        Connector = c.FlashMemoryDetail.Connector,
                        Capacity = c.FlashMemoryDetail.Capacity,
                        Led = c.FlashMemoryDetail.Led,
                        IsImpactResistance = c.FlashMemoryDetail.IsImpactResistance,
                        WaterResistance = c.FlashMemoryDetail.WaterResistance,
                        ShockResistance = c.FlashMemoryDetail.ShockResistance,
                        DustResistance = c.FlashMemoryDetail.DustResistance,
                        AntiScratch = c.FlashMemoryDetail.AntiScratch,
                        AntiStain = c.FlashMemoryDetail.AntiStain,
                        SpeedDataTransfer = c.FlashMemoryDetail.SpeedDataTransfer,
                        SpeedDataReading = c.FlashMemoryDetail.SpeedDataReading,
                        OsCompatibility = c.FlashMemoryDetail.OsCompatibility,
                        MoreInformation = c.FlashMemoryDetail.MoreInformation,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(int productId, string shopperId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.ProductId == productId)
                    .Select(c => new AddOrEditHandsfreeAndHeadPhoneViewModel()
                    {
                       
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditTabletViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        Lenght = c.TabletDetail.Lenght,
                        Width = c.TabletDetail.Width,
                        Height = c.TabletDetail.Height,
                        Weight = c.TabletDetail.Weight,
                        SimCardIsTrue = c.TabletDetail.SimCardIsTrue,
                        Call = c.TabletDetail.Call,
                        SimCardQuantity = c.TabletDetail.SimCardQuantity,
                        SimCardInpute = c.TabletDetail.SimCardInpute,
                        SeparateSlotMemoryCard = c.TabletDetail.SeparateSlotMemoryCard,
                        Announced = c.TabletDetail.Announced,
                        ChipsetName = c.TabletDetail.ChipsetName,
                        Cpu = c.TabletDetail.Cpu,
                        CpuAndFrequency = c.TabletDetail.CpuAndFrequency,
                        CpuArch = c.TabletDetail.CpuArch,
                        Gpu = c.TabletDetail.Gpu,
                        InternalStorage = c.TabletDetail.InternalStorage,
                        Ram = c.TabletDetail.Ram,
                        SdCard = c.TabletDetail.SdCard,
                        SdCardStandard = c.TabletDetail.SdCardStandard,
                        ColorDisplay = c.TabletDetail.ColorDisplay,
                        TouchDisplay = c.TabletDetail.TouchDisplay,
                        DisplayTechnology = c.TabletDetail.DisplayTechnology,
                        DisplaySize = c.TabletDetail.DisplaySize,
                        Resolution = c.TabletDetail.Resolution,
                        PixelDensity = c.TabletDetail.PixelDensity,
                        ScreenToBodyRatio = c.TabletDetail.ScreenToBodyRatio,
                        ImageRatio = c.TabletDetail.ImageRatio,
                        DisplayProtection = c.TabletDetail.DisplayProtection,
                        MoreInformation = c.TabletDetail.MoreInformation,
                        ConnectionsNetwork = c.TabletDetail.ConnectionsNetwork,
                        GsmNetwork = c.TabletDetail.GsmNetwork,
                        HspaNetwork = c.TabletDetail.HspaNetwork,
                        LteNetwork = c.TabletDetail.LteNetwork,
                        FiveGNetwork = c.TabletDetail.FiveGNetwork,
                        CommunicationTechnology = c.TabletDetail.CommunicationTechnology,
                        WiFi = c.TabletDetail.WiFi,
                        Radio = c.TabletDetail.Radio,
                        Bluetooth = c.TabletDetail.Bluetooth,
                        GpsInformation = c.TabletDetail.GpsInformation,
                        ConnectionPort = c.TabletDetail.ConnectionPort,
                        CameraQuantity = c.TabletDetail.CameraQuantity,
                        PhotoResolutation = c.TabletDetail.PhotoResolutation,
                        SelfiCameraPhoto = c.TabletDetail.SelfiCameraPhoto,
                        CameraCapabilities = c.TabletDetail.CameraCapabilities,
                        SelfiCameraCapabilities = c.TabletDetail.SelfiCameraCapabilities,
                        Filming = c.TabletDetail.Filming,
                        Speakers = c.TabletDetail.Speakers,
                        OutputAudio = c.TabletDetail.OutputAudio,
                        AudioInformation = c.TabletDetail.AudioInformation,
                        OS = c.TabletDetail.OS,
                        OsVersion = c.TabletDetail.OsVersion,
                        UiVersion = c.TabletDetail.UiVersion,
                        MoreInformationSoftWare = c.TabletDetail.MoreInformationSoftWare,
                        BatteryMaterial = c.TabletDetail.BatteryMaterial,
                        BatteryCapacity = c.TabletDetail.BatteryCapacity,
                        Removable‌Battery = c.TabletDetail.Removable‌Battery,
                        Sensors = c.TabletDetail.Sensors,
                        ItemsInBox = c.TabletDetail.ItemsInBox,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditSpeakerViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        Lenght = c.SpeakerDetail.Lenght,
                        Width = c.SpeakerDetail.Width,
                        Height = c.SpeakerDetail.Height,
                        ConnectionType = c.SpeakerDetail.ConnectionType,
                        Connector = c.SpeakerDetail.Connector,
                        IsMemoryCardInput = c.SpeakerDetail.IsMemoryCardInput,
                        IsSupportUSBPort = c.SpeakerDetail.IsSupportUSBPort,
                        HeadphoneOutput = c.SpeakerDetail.HeadphoneOutput,
                        InputSound = c.SpeakerDetail.InputSound,
                        MicrophoneInpute = c.SpeakerDetail.MicrophoneInpute,
                        IsSupportMicrophone = c.SpeakerDetail.IsSupportMicrophone,
                        Display = c.SpeakerDetail.Display,
                        ControlRemote = c.SpeakerDetail.ControlRemote,
                        IsSupportRadio = c.SpeakerDetail.IsSupportRadio,
                        Bluetooth = c.SpeakerDetail.Bluetooth,
                        ConnectTwoDevice = c.SpeakerDetail.ConnectTwoDevice,
                        SpeakerItemQuantity = c.SpeakerDetail.SpeakerItemQuantity,
                        IsBattery = c.SpeakerDetail.IsBattery,
                        PlayingTime = c.SpeakerDetail.PlayingTime,
                        ChargingTime = c.SpeakerDetail.ChargingTime,
                        OsSoppurt = c.SpeakerDetail.OsSoppurt,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEdirWristWatchViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditSmartWatchViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        Lenght = c.SmartWatchDetail.Lenght,
                        Width = c.SmartWatchDetail.Width,
                        Height = c.SmartWatchDetail.Height,
                        Weight = c.SmartWatchDetail.Weight,
                        SuitableFor = c.SmartWatchDetail.SuitableFor,
                        Application = c.SmartWatchDetail.Application,
                        DisplayForm = c.SmartWatchDetail.DisplayForm,
                        GlassMaterial = c.SmartWatchDetail.GlassMaterial,
                        CaseMaterial = c.SmartWatchDetail.CaseMaterial,
                        MaterialStrap = c.SmartWatchDetail.MaterialStrap,
                        TypeOfLock = c.SmartWatchDetail.TypeOfLock,
                        ColorDisplay = c.SmartWatchDetail.ColorDisplay,
                        TouchDisplay = c.SmartWatchDetail.TouchDisplay,
                        DisplaySize = c.SmartWatchDetail.DisplaySize,
                        Resolution = c.SmartWatchDetail.Resolution,
                        PixelDensity = c.SmartWatchDetail.PixelDensity,
                        DisplayType = c.SmartWatchDetail.DisplayType,
                        MoreInformationDisplay = c.SmartWatchDetail.MoreInformationDisplay,
                        SimcardIsSoppurt = c.SmartWatchDetail.SimcardIsSoppurt,
                        RegisteredSimCardIsSoppurt = c.SmartWatchDetail.RegisteredSimCardIsSoppurt,
                        GpsIsSoppurt = c.SmartWatchDetail.GpsIsSoppurt,
                        Os = c.SmartWatchDetail.Os,
                        Compatibility = c.SmartWatchDetail.Compatibility,
                        Prossecor = c.SmartWatchDetail.Prossecor,
                        InternalStorage = c.SmartWatchDetail.InternalStorage,
                        ExternalStorageSoppurt = c.SmartWatchDetail.ExternalStorageSoppurt,
                        Camera = c.SmartWatchDetail.Camera,
                        MusicControl = c.SmartWatchDetail.MusicControl,
                        Connections = c.SmartWatchDetail.Connections,
                        Sensors = c.SmartWatchDetail.Sensors,
                        BatteryMaterial = c.SmartWatchDetail.BatteryMaterial,
                        CallIsSoppurt = c.SmartWatchDetail.CallIsSoppurt,
                        MoreInformationHardware = c.SmartWatchDetail.MoreInformationHardware,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditMemoryCardViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Brand = c.BrandId,
                        OfficialBrandProductId = c.OfficialBrandProductId.Value,
                        IsActive = c.IsActive,

                        //img
                        SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                        SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                        SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                        SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                        SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                        SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                        // detail
                        Length = c.MemoryCardDetail.Length,
                        Width = c.MemoryCardDetail.Width,
                        Height = c.MemoryCardDetail.Height,
                        Capacity = c.MemoryCardDetail.Capacity,
                        SpeedStandard = c.MemoryCardDetail.SpeedStandard,
                        ReadingSpeed = c.MemoryCardDetail.ReadingSpeed,
                        ResistsAgainst = c.MemoryCardDetail.ResistsAgainst,
                        MoreInformation = c.MemoryCardDetail.MoreInformation,
                    }).SingleOrDefaultAsync();

        public async Task<AddOrEditAUXViewModel> GetTypeAUXProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                .Select(c => new AddOrEditAUXViewModel()
                {
                    ProductId = c.ProductId,
                    ProductTitle = c.ProductTitle,
                    Description = c.Description,
                    Brand = c.BrandId,
                    OfficialBrandProductId = c.OfficialBrandProductId.Value,
                    IsActive = c.IsActive,

                    //img
                    SelectedImage1IMG = c.ProductGalleries.Skip(0).First().ImageName,
                    SelectedImage2IMG = c.ProductGalleries.Skip(1).First().ImageName,
                    SelectedImage3IMG = c.ProductGalleries.Skip(2).First().ImageName,
                    SelectedImage4IMG = c.ProductGalleries.Skip(3).First().ImageName,
                    SelectedImage5IMG = c.ProductGalleries.Skip(4).First().ImageName,
                    SelectedImage6IMG = c.ProductGalleries.Skip(5).First().ImageName,
                    // detail
                    CableMaterial = c.AuxDetail.CableMaterial,
                    CableLenght = c.AuxDetail.CableLenght
                }).SingleOrDefaultAsync();

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

        public async Task AddPowerBankDetailAsync(PowerBankDetail powerBank)
        {
            await _context.PowerBankDetails.AddAsync(powerBank);
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

        public async Task AddAUXDetailAsync(AUXDetail auxDetail)
            =>
                await _context.AuxDetails.AddAsync(auxDetail);

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

        public void UpdatePowerBankDetail(PowerBankDetail powerBank)
        {
            _context.PowerBankDetails.Update(powerBank);
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

        public void UpdateAUXDetail(AUXDetail auxDetail)
            =>
                _context.AuxDetails.Update(auxDetail);

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