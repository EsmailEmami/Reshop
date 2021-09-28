using Microsoft.EntityFrameworkCore;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Discount;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Discount;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Discount
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ReshopDbContext _context;

        public DiscountRepository(ReshopDbContext context)
        {
            _context = context;
        }

        public Domain.Entities.User.Discount GetDiscountByCode(string discountCode)
            =>
                _context.Discounts.SingleOrDefault(c => c.DiscountCode == discountCode);

        public async Task<Domain.Entities.User.Discount> GetDiscountByCodeAsync(string discountCode)
            =>
                await _context.Discounts.SingleOrDefaultAsync(c => c.DiscountCode == discountCode);

        public void UpdateDiscount(Domain.Entities.User.Discount discount) => _context.Discounts.Update(discount);

        public bool IsUserDiscountCodeExist(string userId, string discountId)
            =>
                _context.UserDiscountCodes.Any(c => c.UserId == userId && c.DiscountId == discountId);

        public async Task AddUserDiscountCodeAsync(UserDiscountCode userDiscountCode)
            =>
                await _context.UserDiscountCodes.AddAsync(userDiscountCode);

        // shopper discount
        public async Task<ShopperProductDiscount> GetLastShopperProductDiscountAsync(string shopperProductColorId) =>
            await _context.ShopperProductDiscounts.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .OrderByDescending(c => c.EndDate).FirstOrDefaultAsync();

        public async Task AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount) =>
            await _context.ShopperProductDiscounts.AddAsync(shopperProductDiscount);

        public void UpdateShopperProductDiscount(ShopperProductDiscount shopperProductDiscount) =>
            _context.ShopperProductDiscounts.Update(shopperProductDiscount);

        public async Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId) =>
            await _context.ShopperProductDiscounts.AnyAsync(c =>
                c.ShopperProductColorId == shopperProductColorId && c.EndDate >= DateTime.Now);

        public IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(
            string shopperProductColorId) =>
            _context.ShopperProductColors
                .Where(c => c.ShopperProductColorId == shopperProductColorId)
                .SelectMany(c => c.Discounts)
                .OrderBy(c => c.StartDate)
                .Take(20)
                .Select(b => new Tuple<string, int>(
                    $"{b.StartDate.ToShamsiDateTime()} تا {b.EndDate.ToShamsiDateTime()}",
                    _context.Orders
                        .Where(or => or.PayDate >= b.StartDate &&
                                     or.PayDate >= b.EndDate)
                        .SelectMany(e => e.OrderDetails)
                        .Count(f => f.ShopperProductColorId == b.ShopperProductColorId &&
                                    f.ProductDiscountPrice != 0)
                ));

        public async Task<DiscountsGeneralDataViewModel> GetShopperProductColorDiscountsGeneralDataAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => new DiscountsGeneralDataViewModel()
                {
                    ProductId = c.ShopperProduct.ProductId,
                    ColorId = c.ColorId,
                    DiscountsCount = c.Discounts.Count,
                    SellCount = _context.OrderDetails.Count(d =>
                        d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0),
                    DiscountedAmount = _context.OrderDetails
                        .Where(d => d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0)
                        .Sum(s => s.ProductDiscountPrice),
                    Income = _context.OrderDetails.Where(d =>
                            d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0)
                        .Sum(s => s.Sum)
                }).SingleOrDefaultAsync();

        public IEnumerable<DiscountsForShowViewModel> GetProductColorDiscountsWithPaginationAsync(int productId, int colorId, int skip, int take, string filter = "")
        {
            IQueryable<ShopperProductColor> discounts = _context.ShopperProductColors
                .Where(c => c.ShopperProduct.ProductId == productId && c.ColorId == colorId);

            if (filter != null)
            {
                discounts = discounts.Where(c => c.ShopperProduct.Product.ProductTitle.Contains(filter));
            }

            return discounts.SelectMany(c => c.Discounts)
                .OrderByDescending(c => c.StartDate)
                .Skip(skip).Take(take)
                .Select(c => new DiscountsForShowViewModel()
                {
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    DiscountedAmount = _context.OrderDetails
                        .Where(d => d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0)
                        .Sum(s => s.ProductDiscountPrice),
                    SellCount = _context.Orders.Where(or => or.PayDate >= c.StartDate && or.PayDate >= c.EndDate)
                        .SelectMany(e => e.OrderDetails).Count(f => f.ShopperProductColorId == c.ShopperProductColorId && f.ProductDiscountPrice != 0),
                    Income = _context.Orders.Where(or => or.PayDate >= c.StartDate && or.PayDate >= c.EndDate)
                        .SelectMany(e => e.OrderDetails)
                        .Where(f => f.ShopperProductColorId == c.ShopperProductColorId && f.ProductDiscountPrice != 0).Sum(os => os.Sum)
                });

        }

        public async Task<int> GetProductColorDiscountsCountAsync(int productId, int colorId) =>
            await _context.ShopperProductColors
                .Where(c => c.ShopperProduct.ProductId == productId && c.ColorId == colorId)
                .CountAsync();

        public IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId) =>
            _context.ShopperProductColors
                .Where(c => c.ShopperProduct.ProductId == productId &&
                            c.ColorId == colorId)
                .SelectMany(c => c.Discounts)
                .OrderBy(c => c.StartDate)
                .Take(20)
                .Select(b => new Tuple<string, int>(
                    $"{b.StartDate.ToShamsiDateTime()} تا {b.EndDate.ToShamsiDateTime()}",
                    _context.Orders
                        .Where(or => or.PayDate >= b.StartDate &&
                                     or.PayDate >= b.EndDate)
                        .SelectMany(e => e.OrderDetails)
                        .Count(f => f.ShopperProductColorId == b.ShopperProductColorId &&
                                    f.ProductDiscountPrice != 0)
                ));

        public async Task<DiscountsGeneralDataViewModel> GetProductColorDiscountsGeneralDataAsync(int productId, int colorId) =>
            await _context.ShopperProductColors
                .Where(c => c.ShopperProduct.ProductId == productId && c.ColorId == colorId)
                .Select(c => new DiscountsGeneralDataViewModel()
                {
                    ProductId = c.ShopperProduct.ProductId,
                    ColorId = c.ColorId,
                    DiscountsCount = c.Discounts.Count,
                    SellCount = _context.OrderDetails.Count(d =>
                        d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0),
                    DiscountedAmount = _context.OrderDetails
                        .Where(d => d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0)
                        .Sum(s => s.ProductDiscountPrice),
                    Income = _context.OrderDetails.Where(d =>
                            d.ShopperProductColorId == c.ShopperProductColorId && d.ProductDiscountPrice != 0)
                        .Sum(s => s.Sum)
                }).SingleOrDefaultAsync();

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}