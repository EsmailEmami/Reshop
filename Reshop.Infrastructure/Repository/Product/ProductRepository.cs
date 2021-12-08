using Microsoft.EntityFrameworkCore;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Product;
using Reshop.Infrastructure.Context;
using System;
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

        public async Task<IEnumerable<SearchProductViewModel>> SearchProductsAsync(string filter) =>
            await _context.Products
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)) &&
                            c.ProductTitle.Contains(filter))
                .Select(c => c.ChildCategory)
                .Distinct()
                .Select(c => new SearchProductViewModel()
                {
                    Title = c.ChildCategoryTitle,
                    Url = $"/ChildCategory/{c.ChildCategoryId}/{c.ChildCategoryTitle.Replace(" ", "-")}?search={filter}"
                }).ToListAsync();


        public async Task<IEnumerable<ProductViewModel>> GetProductsWithPaginationAsync(string type, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Products
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            #region filter product

            if (type == "all")
            {
                products = sortBy switch
                {
                    "news" => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                    "expensive" => products.OrderByDescending(c => c.Price),
                    "cheap" => products.OrderBy(c => c.Price),
                    "mostsale" => products.OrderByDescending(c => c.SaleCount),
                    "mostviews" => products.OrderByDescending(c => c.ViewCount),
                    _ => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate)
                };
            }
            else
            {
                products = sortBy switch
                {
                    "news" => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                    "expensive" => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderByDescending(c => c.Price),
                    "cheap" => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderBy(c => c.Price),
                    "mostsale" => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderByDescending(c => c.SaleCount),
                    "mostviews" => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderByDescending(c => c.ViewCount),
                    _ => products.Where(c => c.ShopperProduct.Product.ProductType.ToLower() == type)
                        .OrderByDescending(c => c.ShopperProduct.Product.CreateDate)
                };
            }

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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return await products.Select(c => new ProductViewModel()
            {
                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                Image = c.ShopperProduct.Product.ProductGalleries
                    .First(i => i.OrderBy == 1).ImageName,
                ProductPrice = c.Price,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId
            }).ToListAsync();
        }

        public IEnumerable<ProductDataForAdmin> GetProductsWithPaginationForAdmin(string type, int skip, int take, string filter)
        {
            IQueryable<Domain.Entities.Product.Product> products = _context.Products;


            #region filter product

            switch (type)
            {
                case "all":
                    {
                        break;
                    }
                case "active":
                    {
                        products = products.Where(c => c.IsActive);
                        break;
                    }
                case "existed":
                    {
                        products = products.Where(c => !c.IsActive);
                        break;
                    }
                default:
                    break;
            }

            if (filter != null)
            {
                products = products.Where(c => c.ProductTitle.Contains(filter));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return products.Select(c => new ProductDataForAdmin()
            {
                ProductId = c.ProductId,
                ProductTitle = c.ProductTitle,
                BrandName = c.OfficialBrandProduct.Brand.BrandName,
                OfficialName = c.OfficialBrandProduct.OfficialBrandProductName
            });
        }

        public IEnumerable<ProductViewModel> GetProductsOfCategoryWithPagination(int categoryId, string sortBy,
            int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0,
            List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .SelectMany(c => c.ChildCategories)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                  .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            #region filter product

            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                "expensive" => products.OrderByDescending(c => c.Price),
                "cheap" => products.OrderBy(c => c.Price),
                "mostsale" => products.OrderByDescending(c => c.SaleCount),
                "mostviews" => products.OrderByDescending(c => c.ViewCount),
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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return products.Select(c => new ProductViewModel()
            {
                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                Image = c.ShopperProduct.Product.ProductGalleries
                    .First(i => i.OrderBy == 1).ImageName,
                ProductPrice = c.Price,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId
            });
        }

        public IEnumerable<ProductViewModel> GetProductsOfChildCategoryWithPagination(int childCategoryId,
            string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0,
            decimal maxPrice = 0, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());


            #region filter product

            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                "expensive" => products.OrderByDescending(c => c.Price),
                "cheap" => products.OrderBy(c => c.Price),
                "mostsale" => products.OrderByDescending(c => c.SaleCount),
                "mostviews" => products.OrderByDescending(c => c.ViewCount),
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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return products.Select(c => new ProductViewModel()
            {
                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                Image = c.ShopperProduct.Product.ProductGalleries
                    .First(i => i.OrderBy == 1).ImageName,
                ProductPrice = c.Price,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId
            });
        }

        public IEnumerable<ProductViewModel> GetProductsOfShopperWithPagination(string shopperId, string sortBy, int skip = 0, int take = 18,
            string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.IsActive &&
                            c.ShopperProductColors
                                .Any(i => i.IsActive))
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(c => c.SaleCount)
                    .First());

            #region filter product

            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                "expensive" => products.OrderByDescending(c => c.Price),
                "cheap" => products.OrderBy(c => c.Price),
                "mostsale" => products.OrderByDescending(c => c.SaleCount),
                "mostviews" => products.OrderByDescending(c => c.ViewCount),
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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return products.Select(c => new ProductViewModel()
            {
                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                Image = c.ShopperProduct.Product.ProductGalleries
                    .First(i => i.OrderBy == 1).ImageName,
                ProductPrice = c.Price,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId
            });
        }

        public IEnumerable<ProductViewModel> GetProductsOfBrandWithPagination(int brandId, string sortBy, int skip = 0, int take = 18, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> officialBrandProducts = null)
        {
            IQueryable<ShopperProductColor> products = _context.Brands
                .Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());


            #region filter product

            products = sortBy switch
            {
                "news" => products.OrderByDescending(c => c.ShopperProduct.Product.CreateDate),
                "expensive" => products.OrderByDescending(c => c.Price),
                "cheap" => products.OrderBy(c => c.Price),
                "mostsale" => products.OrderByDescending(c => c.SaleCount),
                "mostviews" => products.OrderByDescending(c => c.ViewCount),
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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (officialBrandProducts != null && officialBrandProducts.Any())
            {
                products = products
                    .Where(c => officialBrandProducts
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProductId));
            }

            #endregion

            products = products.Skip(skip).Take(take);

            return products.Select(c => new ProductViewModel()
            {

                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                Image = c.ShopperProduct.Product.ProductGalleries
                    .First(i => i.OrderBy == 1).ImageName,
                ProductPrice = c.Price,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId
            });
        }

        public async Task<decimal> GetMaxPriceOfProductsAsync(string type, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products;

            if (type == "all")
            {
                products = _context.Products
                    .Where(c => c.IsActive &&
                                c.ShopperProducts
                                    .Any(i => i.IsActive
                                              && i.ShopperProductColors.Any(s => s.IsActive)))
                    .Select(c => c.ShopperProducts
                        .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                        .First())
                    .Select(c => c.ShopperProductColors
                        .Where(i => i.IsActive)
                        .OrderByDescending(b => b.SaleCount)
                        .First());
            }
            else
            {
                products = _context.Products
                    .Where(c => c.IsActive &&
                                c.ProductType.ToLower() == type &&
                                c.ShopperProducts
                                    .Any(i => i.IsActive
                                              && i.ShopperProductColors.Any(s => s.IsActive)))
                    .Select(c => c.ShopperProducts
                        .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                        .First())
                    .Select(c => c.ShopperProductColors
                        .Where(i => i.IsActive)
                        .OrderByDescending(b => b.SaleCount)
                        .First());
            }

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderByDescending(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMaxPriceOfCategoryProductsAsync(int categoryId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .SelectMany(c => c.ChildCategories)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderByDescending(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMaxPriceOfChildCategoryProductsAsync(int childCategoryId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderByDescending(c => c.Price)
                            .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMaxPriceOfShopperProductsAsync(string shopperId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.IsActive &&
                            c.ShopperProductColors
                                .Any(i => i.IsActive))
                .SelectMany(c => c.ShopperProductColors)
                .Distinct()
                .OrderByDescending(c => c.SaleCount);

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderByDescending(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMaxPriceOfBrandProductsAsync(int brandId, string filter = null, List<int> officialBrandProducts = null)
        {
            IQueryable<ShopperProductColor> products = _context.Brands
                .Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (officialBrandProducts != null && officialBrandProducts.Any())
            {
                products = products
                    .Where(c => officialBrandProducts
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProductId));
            }

            return await products.OrderByDescending(c => c.Price)
                .Select(c => c.Price).FirstAsync();
        }

        public async Task<decimal> GetMinPriceOfProductsAsync(string type, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products;

            if (type == "all")
            {
                products = _context.Products
                    .Where(c => c.IsActive &&
                                c.ShopperProducts
                                    .Any(i => i.IsActive
                                              && i.ShopperProductColors.Any(s => s.IsActive)))
                    .Select(c => c.ShopperProducts
                        .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                        .First())
                    .Select(c => c.ShopperProductColors
                        .Where(i => i.IsActive)
                        .OrderByDescending(b => b.SaleCount)
                        .First());
            }
            else
            {
                products = _context.Products
                    .Where(c => c.IsActive &&
                                c.ProductType.ToLower() == type &&
                                c.ShopperProducts
                                    .Any(i => i.IsActive
                                              && i.ShopperProductColors.Any(s => s.IsActive)))
                    .Select(c => c.ShopperProducts
                        .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                        .First())
                    .Select(c => c.ShopperProductColors
                        .Where(i => i.IsActive)
                        .OrderByDescending(b => b.SaleCount)
                        .First());
            }

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderBy(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMinPriceOfCategoryProductsAsync(int categoryId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .SelectMany(c => c.ChildCategories)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderBy(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMinPriceOfChildCategoryProductsAsync(int childCategoryId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderBy(c => c.Price)
                            .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMinPriceOfShopperProductsAsync(string shopperId, string filter = null, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.IsActive &&
                            c.ShopperProductColors
                                .Any(i => i.IsActive))
                .SelectMany(c => c.ShopperProductColors)
                .Distinct()
                .OrderByDescending(c => c.SaleCount);

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.OrderBy(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetMinPriceOfBrandProductsAsync(int brandId, string filter = null, List<int> officialBrandProducts = null)
        {
            IQueryable<ShopperProductColor> products = _context.Brands
                .Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

            if (filter != null)
            {
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (officialBrandProducts != null && officialBrandProducts.Any())
            {
                products = products
                    .Where(c => officialBrandProducts
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProductId));
            }

            return await products.OrderBy(c => c.Price)
                .Select(c => c.Price).FirstAsync();
        }

        public async Task<string> GetProductFirstPictureName(int productId)
        {
            return await _context.ProductGalleries
                .Where(c => c.ProductId == productId).Select(c => c.ImageName).FirstOrDefaultAsync();
        }

        public async Task<ProductDataForDetailViewModel> GetProductDataForDetailAsync(string shopperProductColorId)
        {
            var model = await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                    .Select(c => new ProductDataForDetailViewModel()
                    {
                        ShopperProductColorId = c.ShopperProductColorId,
                        ShortKey = c.ShortKey,
                        SelectedColor = c.ColorId,
                        ProductId = c.ShopperProduct.ProductId,
                        Title = c.ShopperProduct.Product.ProductTitle,
                        Description = c.ShopperProduct.Product.Description,
                        Price = c.Price,
                        Type = c.ShopperProduct.Product.ProductType,
                        Brand = new Tuple<int, string>(c.ShopperProduct.Product.OfficialBrandProduct.BrandId,
                            c.ShopperProduct.Product.OfficialBrandProduct.Brand.BrandName),
                        LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                            .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                            .FirstOrDefault(),
                    }).SingleOrDefaultAsync();




            object detail = new object();


            switch (model.Type.ToLower())
            {
                case "mobile":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.MobileDetail).SingleOrDefaultAsync();
                        break;
                    }
                case "laptop":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.LaptopDetail).SingleOrDefaultAsync();
                        break;
                    }
                case "aux":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.AuxDetail).SingleOrDefaultAsync();
                        break;
                    }
            }


            model.Detail = detail;


            return model;
        }

        public async Task<Tuple<string, string>> GetProductRedirectionByShortKeyAsync(string key)
            => await _context.ShopperProductColors.Where(c => c.ShortKey == key)
                .Select(c => new Tuple<string, string>(c.ShopperProduct.Product.ProductTitle, c.ShopperProductColorId))
                .SingleOrDefaultAsync();

        public async Task<Tuple<string, string>> GetBestSellerOfProductAsync(int productId) =>
            await _context.Products.Where(c => c.ProductId == productId && c.IsActive &&
                                               c.ShopperProducts
                                                   .Any(i => i.IsActive
                                                             && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(v => v.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(v => v.SaleCount)
                    .First())
                .Select(c => new Tuple<string, string>(c.ShopperProduct.Product.ProductTitle, c.ShopperProductColorId))
                .FirstOrDefaultAsync();

        public async Task<ProductDetailForShow> GetProductDetailForShopperAsync(string shopperProductId) =>
            await _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                .Select(c => new ProductDetailForShow()
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductTitle,
                    ProductImages = c.Product.ProductGalleries.OrderBy(i => i.OrderBy).Select(i => i.ImageName).ToList(),
                    BrandName = c.Product.OfficialBrandProduct.Brand.BrandName,
                    ShoppersCount = c.Product.ShopperProducts.Count,
                    Colors = c.ShopperProductColors.Select(c => new Tuple<int, string>(c.ColorId, c.Color.ColorName))
                }).SingleOrDefaultAsync();

        public async Task<ProductDetailForShow> GetProductDetailForAdminAsync(int productId) =>
            await _context.Products.Where(c => c.ProductId == productId)
                .Select(c => new ProductDetailForShow()
                {
                    ProductId = c.ProductId,
                    ProductName = c.ProductTitle,
                    ProductImages = c.ProductGalleries.OrderBy(i => i.OrderBy).Select(i => i.ImageName).ToList(),
                    BrandName = $"{c.OfficialBrandProduct.OfficialBrandProductName} ({c.OfficialBrandProduct.Brand.BrandName})",
                    ShoppersCount = c.ShopperProducts.Count,
                    ProductScore = 4.4
                }).SingleOrDefaultAsync();

        public async Task<ProductDataForCompareViewModel> GetProductDataForCompareAsync(int productId)
        {
            var model = await _context.Products.Where(c => c.ProductId == productId)
                   .Select(c => new ProductDataForCompareViewModel()
                   {
                       ProductId = c.ProductId,
                       Title = c.ProductTitle,
                       Type = c.ProductType,
                       ImageName = c.ProductGalleries
                           .First(i => i.OrderBy == 1).ImageName
                   }).SingleOrDefaultAsync();

            object detail = new object();

            switch (model.Type.ToLower())
            {
                case "mobile":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.MobileDetail).SingleOrDefaultAsync();
                        break;
                    }
                case "laptop":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.LaptopDetail).SingleOrDefaultAsync();
                        break;
                    }
                case "aux":
                    {
                        detail = await _context.Products.Where(c => c.ProductId == model.ProductId)
                            .Select(c => c.AuxDetail).SingleOrDefaultAsync();
                        break;
                    }
            }

            model.Detail = detail;

            return model;
        }

        public IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId, int colorId) =>
            _context.ShopperProducts.Where(c => c.ProductId == productId)
                .SelectMany(c => c.ShopperProductColors)
                .Where(c => c.ColorId == colorId)
                .Select(c => new Tuple<string, string, string>(
                    c.ShopperProductColorId,
                    c.ShopperProduct.Shopper.StoreName,
                    c.ShopperProduct.Warranty));

        public async Task<Domain.Entities.Product.Product> GetProductByIdAsync(int productId) =>
            await _context.Products.FindAsync(productId);

        public async Task<EditProductDetailShopperViewModel> EditProductDetailShopperAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors
                .Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => new EditProductDetailShopperViewModel()
                {
                    ShopperId = c.ShopperProduct.ShopperId,
                    StoreName = c.ShopperProduct.Shopper.StoreName,
                    ShortKey = c.ShortKey,
                    SelectedShopper = c.ShopperProductColorId,
                    SelectedColor = c.ColorId,
                    Price = c.Price,
                    ProductTitle = c.ShopperProduct.Product.ProductTitle,
                    LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                        .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                        .FirstOrDefault(),
                }).SingleOrDefaultAsync();

        public Task<int> GetProductIdOfShopperProductColorIdAsync(string shopperProductColorId) =>
            _context.ShopperProductColors
                .Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => c.ShopperProduct.ProductId)
                .FirstOrDefaultAsync();

        public async Task<int> GetProductsCountWithTypeAsync(string type = "all")
        {
            return type switch
            {
                "all" => await _context.Products.CountAsync(),
                "active" => await _context.Products.Where(c => c.IsActive).CountAsync(),
                "non-active" => await _context.Products.Where(c => !c.IsActive).CountAsync(),
                _ => await _context.Products.Where(c => c.ProductType == type).CountAsync()
            };
        }

        public async Task<int> GetUserFavoriteProductsCountWithTypeAsync(string userId) =>
            await _context.FavoriteProducts.Where(c => c.UserId == userId)
                .Select(c => c.ShopperProductColor)
                .Where(c => c.IsActive)
                .CountAsync();

        public async Task<int> GetProductsCountAsync(string type = "all", string filter = null, decimal minPrice = 0, decimal maxPrice = 0,
            List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Products
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors.Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.CountAsync();
        }


        public async Task<int> GetCategoryProductsCountAsync(int categoryId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .SelectMany(c => c.ChildCategories)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts
                                .Any(i => i.IsActive
                                          && i.ShopperProductColors.Any(s => s.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.CountAsync();
        }

        public async Task<int> GetChildCategoryProductsCountAsync(int childCategoryId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ChildCategories
                .Where(c => c.ChildCategoryId == childCategoryId)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts.Any(s => s.IsActive &&
                                                       s.ShopperProductColors.Any(i => i.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.CountAsync();
        }

        public async Task<int> GetShopperProductsCountAsync(string shopperId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0,
            List<int> brands = null)
        {
            IQueryable<ShopperProductColor> products = _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.IsActive && c.ShopperProductColors
                    .Any(s => s.IsActive))
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());


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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (brands != null && brands.Any())
            {
                products = products
                    .Where(c => brands
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProduct.BrandId));
            }

            return await products.CountAsync();
        }

        public async Task<int> GetBrandProductsCountAsync(int brandId, string filter = null, decimal minPrice = 0, decimal maxPrice = 0, List<int> officialBrandProducts = null)
        {
            IQueryable<ShopperProductColor> products = _context.Brands
                .Where(c => c.BrandId == brandId)
                .SelectMany(c => c.OfficialBrandProducts)
                .SelectMany(c => c.Products)
                .Where(c => c.IsActive &&
                            c.ShopperProducts.Any(s => s.IsActive &&
                                                       s.ShopperProductColors.Any(i => i.IsActive)))
                .Select(c => c.ShopperProducts
                    .OrderByDescending(b => b.ShopperProductColors
                        .Sum(s => s.SaleCount))
                    .First())
                .Select(c => c.ShopperProductColors
                    .Where(i => i.IsActive)
                    .OrderByDescending(b => b.SaleCount)
                    .First());

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
                products = products.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            if (officialBrandProducts != null && officialBrandProducts.Any())
            {
                products = products
                    .Where(c => officialBrandProducts
                        .Any(b => b == c.ShopperProduct.Product.OfficialBrandProductId));
            }

            return await products.CountAsync();
        }

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperId, int productId) =>
            await _context.ShopperProducts
                .Where(c => c.ShopperId == shopperId && c.ProductId == productId).SingleOrDefaultAsync();

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperProductId) =>
             await _context.ShopperProducts.Where(c => c.ShopperProductId == shopperProductId)
                    .Include(c => c.Product).SingleOrDefaultAsync();

        public async Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductId, int colorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductId == shopperProductId && c.ColorId == colorId)
                .Select(c => new EditColorOfShopperProductViewModel()
                {
                    ShopperProductColorId = c.ShopperProductColorId,
                    QuantityInStock = c.QuantityInStock,
                    Price = c.Price,
                    IsActive = c.IsActive
                }).SingleOrDefaultAsync();

        public async Task<EditColorOfShopperProductViewModel> GetShopperProductColorForEditAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => new EditColorOfShopperProductViewModel()
                {
                    ShopperProductColorId = c.ShopperProductColorId,
                    QuantityInStock = c.QuantityInStock,
                    Price = c.Price,
                    IsActive = c.IsActive
                }).SingleOrDefaultAsync();

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

        public IEnumerable<ProductGallery> GetProductImages(int productId)
        {
            return _context.ProductGalleries.Where(c => c.ProductId == productId)
                .OrderBy(c => c.OrderBy);
        }


        public IEnumerable<ProductViewModel> GetUserFavoriteProductsWithPagination(string userId, string sortBy, int skip = 0, int take = 24)
        {
            IQueryable<ShopperProductColor> favoriteProducts = _context.FavoriteProducts
                .Where(c => c.UserId == userId)
                .Select(c => c.ShopperProductColor)
                .Where(c => c.IsActive);


            #region filter product

            favoriteProducts = sortBy switch
            {
                "news" => favoriteProducts.OrderByDescending(c => c.CreateDate),
                "expensive" => favoriteProducts.OrderByDescending(c => c.Price),
                "cheap" => favoriteProducts.OrderBy(c => c.Price),
                "mostsale" => favoriteProducts.OrderByDescending(c => c.SaleCount),
                "mostviews" => favoriteProducts.OrderByDescending(c => c.ViewCount),
                _ => favoriteProducts.OrderByDescending(c => c.CreateDate),
            };

            #endregion

            favoriteProducts = favoriteProducts.Skip(skip).Take(take);

            return favoriteProducts.Select(c => new ProductViewModel()
            {
                ProductTitle = c.ShopperProduct.Product.ProductTitle,
                LastDiscount = c.Discounts.Where(d => d.StartDate < DateTime.Now && d.EndDate > DateTime.Now)
                    .Select(t => new Tuple<byte, DateTime>(t.DiscountPercent, t.EndDate))
                    .FirstOrDefault(),
                ProductPrice = c.Price,
                ShopperProductColorId = c.ShopperProductColorId,
                ProductId = c.ShopperProduct.ProductId,
                Image = c.ShopperProduct.Product.ProductGalleries.OrderBy(g => g.OrderBy).First().ImageName
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

        public async Task<bool> IsFavoriteProductExistAsync(string userId, string shopperProductColorId) =>
            await _context.FavoriteProducts.AnyAsync(c => c.UserId == userId && c.ShopperProductColorId == shopperProductColorId);

        public async Task<FavoriteProduct> GetFavoriteProductAsync(string favoriteProductId)
        {
            return await _context.FavoriteProducts.FindAsync(favoriteProductId);
        }

        public async Task<FavoriteProduct> GetFavoriteProductAsync(string userId, string shopperProductColorId) =>
            await _context.FavoriteProducts.SingleOrDefaultAsync(c => c.UserId == userId && c.ShopperProductColorId == shopperProductColorId);

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductsDataChart() =>
            _context.OrderDetails
                .Where(c => c.Order.IsPayed &&
                            c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .OrderBy(c => c.Order.PayDate)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Order.PayDate.Value.ToShamsiDate(),
                    ViewCount = 10,
                    SellCount = c.Count,
                });

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(int productId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .OrderBy(c => c.Order.PayDate)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Order.PayDate.Value.ToShamsiDate(),
                    ViewCount = 10,
                    SellCount = c.Count,
                });

        public async Task<ProductGallery> GetProductGalleryAsync(int productId, string imageName)
            =>
                await _context.ProductGalleries.SingleOrDefaultAsync(c => c.ProductId == productId && c.ImageName == imageName);

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
                    OfficialBrandProductId = c.OfficialBrandProductId,
                    IsActive = c.IsActive,
                    SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                    SelectedBrand = c.OfficialBrandProduct.BrandId,
                    SelectedChildCategory = c.ChildCategoryId,
                    //img
                    SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                    SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                    SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                    SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                    SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                    SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                     OfficialBrandProductId = c.OfficialBrandProductId,
                     IsActive = c.IsActive,
                     SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                     SelectedBrand = c.OfficialBrandProduct.BrandId,
                     SelectedChildCategory = c.ChildCategoryId,
                     //img
                     SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                     SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                     SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                     SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                     SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                     SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                     OfficialBrandProductId = c.OfficialBrandProductId,
                     IsActive = c.IsActive,
                     SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                     SelectedBrand = c.OfficialBrandProduct.BrandId,
                     SelectedChildCategory = c.ChildCategoryId,
                     //img
                     SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                     SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                     SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                     SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                     SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                     SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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

        public async Task<AddOrEditTabletViewModel> GetTypeTabletProductDataForEditAsync(int productId) =>
            await _context.Products
                .Where(c => c.ProductId == productId)
                    .Select(c => new AddOrEditTabletViewModel()
                    {
                        ProductId = c.ProductId,
                        ProductTitle = c.ProductTitle,
                        Description = c.Description,
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                        OfficialBrandProductId = c.OfficialBrandProductId,
                        IsActive = c.IsActive,
                        SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                        SelectedBrand = c.OfficialBrandProduct.BrandId,
                        SelectedChildCategory = c.ChildCategoryId,
                        //img
                        SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                        SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                        SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                        SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                        SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                        SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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
                    OfficialBrandProductId = c.OfficialBrandProductId,
                    IsActive = c.IsActive,
                    SelectedStoreTitle = c.OfficialBrandProduct.Brand.StoreTitleId,
                    SelectedBrand = c.OfficialBrandProduct.BrandId,
                    SelectedChildCategory = c.ChildCategoryId,
                    //img
                    SelectedImage1IMG = c.ProductGalleries.First(i => i.OrderBy == 1).ImageName,
                    SelectedImage2IMG = c.ProductGalleries.First(i => i.OrderBy == 2).ImageName,
                    SelectedImage3IMG = c.ProductGalleries.First(i => i.OrderBy == 3).ImageName,
                    SelectedImage4IMG = c.ProductGalleries.First(i => i.OrderBy == 4).ImageName,
                    SelectedImage5IMG = c.ProductGalleries.First(i => i.OrderBy == 5).ImageName,
                    SelectedImage6IMG = c.ProductGalleries.First(i => i.OrderBy == 6).ImageName,
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

        public void RemoveProductGallery(ProductGallery productGallery)
            => _context.ProductGalleries.Remove(productGallery);

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
            return await _context.ShopperProductColors.AnyAsync(c => c.ShortKey == shortKey);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}