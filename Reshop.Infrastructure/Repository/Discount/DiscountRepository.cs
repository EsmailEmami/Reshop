using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Discount;
using Reshop.Infrastructure.Context;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;

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

        public async Task<ShopperProductColorDiscountDetailViewModel> GetShopperProductColorDiscountDetailAsync(string shopperProductColorId) =>
            await _context.ShopperProductColors.Where(c => c.ShopperProductColorId == shopperProductColorId)
                .Select(c => new ShopperProductColorDiscountDetailViewModel()
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
                        .Sum(s => s.Sum),
                    Discounts = c.Discounts.OrderByDescending(a => a.EndDate)
                        .Select(b => new Tuple<DateTime, DateTime, int, decimal>
                        (
                            b.StartDate,
                            b.EndDate,
                            _context.Orders.Where(or => or.PayDate >= b.StartDate && or.PayDate >= b.EndDate)
                                .SelectMany(e => e.OrderDetails).Count(f => f.ShopperProductColorId == c.ShopperProductColorId && f.ProductDiscountPrice != 0),
                            _context.Orders.Where(or => or.PayDate >= b.StartDate && or.PayDate >= b.EndDate)
                                .SelectMany(e => e.OrderDetails)
                                .Where(f => f.ShopperProductColorId == c.ShopperProductColorId && f.ProductDiscountPrice != 0).Sum(os => os.Sum)
))
                }).SingleOrDefaultAsync();

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

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}