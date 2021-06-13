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

        public IAsyncEnumerable<ProductViewModel> GetProducts()
        {
            return _context.Products.Select(c => new ProductViewModel()
            {
                ProductId = c.ProductId,
                ProductPrice = c.Price,
                ProductTitle = c.ProductTitle,
                BrandName = c.Brand
            }) as IAsyncEnumerable<ProductViewModel>;
        }

        public IAsyncEnumerable<ProductViewModel> GetProductsWithPagination(string type, string sortBy, int skip, int take)
        {
            if (type == "all")
            {
                return sortBy switch
                {
                    "news" => _context.Products
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.CreateDate)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "expensive" => _context.Products
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Price)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "cheap" => _context.Products
                        .Skip(skip).Take(take)
                        .OrderBy(c => c.Price)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostsale" => _context.Products
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.AllSalesCount)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostviews" => _context.Products
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.AllViewsCount)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,

                    _ => _context.Products
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.ProductId)
                        .Include(c => c.ShopperProducts)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Price,
                            ProductTitle = c.ProductTitle,
                            BrandName = c.Brand,
                            ShopperUserId = c.ShopperProducts.First().ShopperUserId
                        }) as IAsyncEnumerable<ProductViewModel>,
                };
            }

            return sortBy switch
            {
                "news" => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.ProductId)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,

                "expensive" => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.Price)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,

                "cheap" => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderBy(c => c.Price)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,

                "mostsale" => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.AllSalesCount)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,

                "mostviews" => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.AllViewsCount)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,

                _ => _context.Products.Where(c => c.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.ProductId)
                    .Include(c => c.ShopperProducts)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Price,
                        ProductTitle = c.ProductTitle,
                        BrandName = c.Brand,
                        ShopperUserId = c.ShopperProducts.First().ShopperUserId
                    }) as IAsyncEnumerable<ProductViewModel>,
            };
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
                "expensive" => products.OrderByDescending(c => c.Price),
                "cheap" => products.OrderBy(c => c.Price),
                "mostsale" => products.OrderByDescending(c => c.AllSalesCount),
                "mostviews" => products.OrderByDescending(c => c.AllViewsCount),
                _ => products
            };



            if (minPrice != 0)
            {
                products = products.Where(c => c.Price >= minPrice);
            }

            if (maxPrice != 0)
            {
                products = products.Where(c => c.Price <= maxPrice);
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
                ProductPrice = c.Price,
                ShopperUserId = c.ShopperProducts.First().ShopperUserId
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



        public async Task<Domain.Entities.Product.Product> GetProductByShortKeyAsync(string key)
            =>
                await _context.Products.SingleOrDefaultAsync(c => c.ShortKey == key);

        public IAsyncEnumerable<ProductViewModel> GetShopperProductsWthPagination(string shopperUserId, string type, string sortBy, int skip, int take)
        {
            try
            {
                if (type == "all")
                {
                    return sortBy switch
                    {
                        "news" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderByDescending(c => c.Product.CreateDate)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,

                        "expensive" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderByDescending(c => c.Product.Price)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,

                        "cheap" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderBy(c => c.Product.CreateDate)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,

                        "mostsale" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderByDescending(c => c.Product.AllSalesCount)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,

                        "mostviews" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderByDescending(c => c.Product.AllViewsCount)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,

                        _ => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                            .Skip(skip).Take(take)
                            .OrderByDescending(c => c.Product.CreateDate)
                            .Select(c => new ProductViewModel()
                            {
                                ProductId = c.Product.ProductId,
                                ProductTitle = c.Product.ProductTitle,
                                ProductPrice = c.Product.Price,
                                BrandName = c.Product.Brand
                            }) as IAsyncEnumerable<ProductViewModel>,
                    };
                }

                return sortBy switch
                {
                    "news" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Product.CreateDate)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "expensive" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Product.Price)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "cheap" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderBy(c => c.Product.Price)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostsale" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Product.AllSalesCount)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostviews" => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Product.AllViewsCount)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    _ => _context.ShopperProducts.Where(c => c.ShopperUserId == shopperUserId)
                        .Skip(skip).Take(take)
                        .OrderByDescending(c => c.Product.CreateDate)
                        .Where(c => c.Product.ProductType == type)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.Product.ProductId,
                            ProductTitle = c.Product.ProductTitle,
                            ProductPrice = c.Product.Price,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,
                };
            }
            catch
            {
                return null;
            }

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

        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId)
            =>
                await _context.Products.SingleOrDefaultAsync(c => c.ProductId == productId);

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

        public IAsyncEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string type, string sortBy, int skip = 0, int take = 24)
        {
            if (type == "all")
            {
                return sortBy switch
                {
                    "news" => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderByDescending(c => c.ProductId)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "expensive" => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderByDescending(c => c.Product.Price)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "cheap" => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderBy(c => c.Product.Price)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostsale" => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderByDescending(c => c.Product.AllSalesCount)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    "mostviews" => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderByDescending(c => c.Product.AllViewsCount)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,

                    _ => _context.FavoriteProducts
                        .Where(c => c.UserId == userId)
                        .Skip(skip).Take(take)
                        .Include(c => c.Product)
                        .OrderByDescending(c => c.ProductId)
                        .Select(c => new ProductViewModel()
                        {
                            ProductId = c.ProductId,
                            ProductPrice = c.Product.Price,
                            ProductTitle = c.Product.ProductTitle,
                            BrandName = c.Product.Brand
                        }) as IAsyncEnumerable<ProductViewModel>,
                };
            }


            return sortBy switch
            {
                "news" => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.ProductId)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,

                "expensive" => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.Product.Price)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,

                "cheap" => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderBy(c => c.Product.Price)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,

                "mostsale" => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.Product.AllSalesCount)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,

                "mostviews" => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.Product.AllViewsCount)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,

                _ => _context.FavoriteProducts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Product)
                    .Where(c => c.Product.ProductType == type)
                    .Skip(skip).Take(take)
                    .OrderByDescending(c => c.ProductId)
                    .Select(c => new ProductViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductPrice = c.Product.Price,
                        ProductTitle = c.Product.ProductTitle,
                        BrandName = c.Product.Brand
                    }) as IAsyncEnumerable<ProductViewModel>,
            };
        }

        public async Task AddToFavoriteProductAsync(FavoriteProduct favoriteProduct)
        {
            await _context.FavoriteProducts.AddAsync(favoriteProduct);
        }

        public void RemoveFavoriteProduct(FavoriteProduct favoriteProduct)
        {
            _context.FavoriteProducts.Remove(favoriteProduct);
        }

        public async Task<bool> IsFavoriteProductExistAsync(string favoriteProductId)
        {
            return await _context.FavoriteProducts.AnyAsync(c => c.FavoriteProductId == favoriteProductId);
        }

        public async Task<FavoriteProduct> GetFavoriteProductByIdAsync(string favoriteProductId)
        {
            return await _context.FavoriteProducts.FindAsync(favoriteProductId);
        }

        public IAsyncEnumerable<string> GetProductsNameByFilter(string productName)
        {
            return _context.Products.Where(c => c.ProductTitle.Contains(productName))
                .Select(c => c.ProductTitle) as IAsyncEnumerable<string>;
        }

        public IAsyncEnumerable<ProductViewModel> GetProductsByFilter(string productName)
        {
            return _context.Products.Where(c => c.ProductTitle.Contains(productName))
                .Select(c => new ProductViewModel()
                {
                    ProductId = c.ProductId,
                    ProductPrice = c.Price,
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

        public async Task<AddOrEditMobileProductViewModel> GetTypeMobileProductDataForEditAsync(int productId)
        {
            return await _context.Products
                .Include(c => c.MobileDetail)
                .Select(c => new AddOrEditMobileProductViewModel()
                {
                    ProductId = c.ProductId,
                    ProductTitle = c.ProductTitle,
                    Description = c.Description,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock,
                    BrandProduct = c.BrandProduct,
                    ProductBrand = c.Brand,
                    InternalMemory = c.MobileDetail.InternalMemory,
                    CommunicationNetworks = c.MobileDetail.CommunicationNetworks,
                    BackCameras = c.MobileDetail.BackCameras,
                    OperatingSystem = c.MobileDetail.OperatingSystem,
                    SIMCardDescription = c.MobileDetail.SIMCardDescription,
                    RAMValue = c.MobileDetail.RAMValue,
                    PhotoResolution = c.MobileDetail.PhotoResolution,
                    OperatingSystemVersion = c.MobileDetail.OperatingSystemVersion,
                    DisplayTechnology = c.MobileDetail.DisplayTechnology,
                    Features = c.MobileDetail.Features,
                    Size = c.MobileDetail.Size,
                    QuantitySIMCard = c.MobileDetail.QuantitySIMCard,
                }).SingleOrDefaultAsync(c => c.ProductId == productId);
        }

        public async Task<AddOrEditLaptopProductViewModel> GetTypeLaptopProductDataForEditAsync(int productId)
        {
            return await _context.Products
                .Include(c => c.LaptopDetail)
                .Select(c => new AddOrEditLaptopProductViewModel()
                {
                    ProductId = c.ProductId,
                    ProductTitle = c.ProductTitle,
                    Description = c.Description,
                    Price = c.Price,
                    QuantityInStock = c.QuantityInStock,
                    BrandProduct = c.BrandProduct,
                    ProductBrand = c.Brand,
                    RAMCapacity = c.LaptopDetail.RAMCapacity,
                    InternalMemory = c.LaptopDetail.InternalMemory,
                    GPUManufacturer = c.LaptopDetail.GPUManufacturer,
                    Size = c.LaptopDetail.Size,
                    Category = c.LaptopDetail.Category,
                    ProcessorSeries = c.LaptopDetail.ProcessorSeries,
                    RAMType = c.LaptopDetail.RAMType,
                    ScreenAccuracy = c.LaptopDetail.ScreenAccuracy,
                    IsMatteScreen = c.LaptopDetail.IsMatteScreen,
                    IsTouchScreen = c.LaptopDetail.IsTouchScreen,
                    OperatingSystem = c.LaptopDetail.OperatingSystem,
                    IsHDMIPort = c.LaptopDetail.IsHDMIPort,
                }).SingleOrDefaultAsync(c => c.ProductId == productId);
        }

        public async Task<AddOrEditMobileCoverViewModel> GetTypeMobileCoverProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.MobileCoverDetail)
                    .Select(c => new AddOrEditMobileCoverViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        SuitablePhones = c.MobileCoverDetail.SuitablePhones,
                        Gender = c.MobileCoverDetail.Gender,
                        Structure = c.MobileCoverDetail.Structure,
                        CoverLevel = c.MobileCoverDetail.CoverLevel,
                        Features = c.MobileCoverDetail.Features
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditFlashMemoryViewModel> GetTypeFlashMemoryProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.FlashMemoryDetail)
                    .Select(c => new AddOrEditFlashMemoryViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        Connector = c.FlashMemoryDetail.Connector,
                        Capacity = c.FlashMemoryDetail.Capacity,
                        IsImpactResistance = c.FlashMemoryDetail.IsImpactResistance
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditHandsfreeAndHeadPhoneViewModel> GetTypeHandsfreeAndHeadPhoneProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.HandsfreeAndHeadPhoneDetail)
                    .Select(c => new AddOrEditHandsfreeAndHeadPhoneViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        ConnectionType = c.HandsfreeAndHeadPhoneDetail.ConnectionType,
                        PhoneType = c.HandsfreeAndHeadPhoneDetail.PhoneType,
                        WorkSuggestion = c.HandsfreeAndHeadPhoneDetail.WorkSuggestion,
                        Connector = c.HandsfreeAndHeadPhoneDetail.Connector,
                        IsSupportBattery = c.HandsfreeAndHeadPhoneDetail.IsSupportBattery,
                        Features = c.HandsfreeAndHeadPhoneDetail.Features
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.TabletDetail)
                    .Select(c => new AddOrEditTabletViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        InternalMemory = c.TabletDetail.InternalMemory,
                        RAMValue = c.TabletDetail.RAMValue,
                        IsTalkAbility = c.TabletDetail.IsTalkAbility,
                        Size = c.TabletDetail.Size,
                        CommunicationNetworks = c.TabletDetail.CommunicationNetworks,
                        Features = c.TabletDetail.Features,
                        IsSIMCardSupporter = c.TabletDetail.IsSIMCardSupporter,
                        QuantitySIMCard = c.TabletDetail.QuantitySIMCard,
                        OperatingSystemVersion = c.TabletDetail.OperatingSystemVersion,
                        CommunicationTechnologies = c.TabletDetail.CommunicationTechnologies,
                        CommunicationPorts = c.TabletDetail.CommunicationPorts
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditSpeakerViewModel> GetTypeSpeakerProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.SpeakerDetail)
                    .Select(c => new AddOrEditSpeakerViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        ConnectionType = c.SpeakerDetail.ConnectionType,
                        Connector = c.SpeakerDetail.Connector,
                        BluetoothVersion = c.SpeakerDetail.BluetoothVersion,
                        IsMemoryCardInput = c.SpeakerDetail.IsMemoryCardInput,
                        IsSupportBattery = c.SpeakerDetail.IsSupportBattery,
                        IsSupportMicrophone = c.SpeakerDetail.IsSupportMicrophone,
                        IsSupportUSBPort = c.SpeakerDetail.IsSupportUSBPort,
                        IsSupportRadio = c.SpeakerDetail.IsSupportRadio
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEdirWristWatchViewModel> GetTypeWristWatchProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.WristWatchDetail)
                    .Select(c => new AddOrEdirWristWatchViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        IsSupportGPS = c.WristWatchDetail.IsSupportGPS,
                        IsTouchScreen = c.WristWatchDetail.IsTouchScreen,
                        WatchForm = c.WristWatchDetail.WatchForm
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditSmartWatchViewModel> GetTypeSmartWatchProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.SmartWatchDetail)
                    .Select(c => new AddOrEditSmartWatchViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        IsSuitableForMen = c.SmartWatchDetail.IsSuitableForMen,
                        IsSuitableForWomen = c.SmartWatchDetail.IsSuitableForWomen,
                        IsScreenColorful = c.SmartWatchDetail.IsScreenColorful,
                        IsSIMCardSupporter = c.SmartWatchDetail.IsSIMCardSupporter,
                        IsTouchScreen = c.SmartWatchDetail.IsTouchScreen,
                        IsSupportSIMCardRegister = c.SmartWatchDetail.IsSupportSIMCardRegister,
                        WorkSuggestion = c.SmartWatchDetail.WorkSuggestion,
                        IsSupportGPS = c.SmartWatchDetail.IsSupportGPS,
                        WatchForm = c.SmartWatchDetail.WatchForm,
                        BodyMaterial = c.SmartWatchDetail.BodyMaterial,
                        Connections = c.SmartWatchDetail.Connections,
                        Sensors = c.SmartWatchDetail.Sensors,
                        IsDirectTalkable = c.SmartWatchDetail.IsDirectTalkable,
                        IsTalkableWithBluetooth = c.SmartWatchDetail.IsTalkableWithBluetooth
                    }).SingleOrDefaultAsync(c => c.ProductId == productId);

        public async Task<AddOrEditMemoryCardViewModel> GetTypeMemoryCardProductDataForEditAsync(int productId)
            =>
                await _context.Products.Include(c => c.SmartWatchDetail)
                    .Select(c => new AddOrEditMemoryCardViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        Price = c.Price,
                        QuantityInStock = c.QuantityInStock,
                        BrandProduct = c.BrandProduct,
                        ProductBrand = c.Brand,
                        Capacity = c.MemoryCardDetail.Capacity,
                        Size = c.MemoryCardDetail.Size,
                        SpeedStandard = c.MemoryCardDetail.SpeedStandard,
                        ResistsAgainst = c.MemoryCardDetail.ResistsAgainst
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