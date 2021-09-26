using Microsoft.EntityFrameworkCore;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.Interfaces.Product;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Product
{
    public class ColorRepository : IColorRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public ColorRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public IEnumerable<Tuple<int, string, string, string>> GetProductColorsWithDetail(int productId) =>
            _context.ShopperProducts
                .Where(c => c.IsActive && c.ProductId == productId)
                .SelectMany(c => c.ShopperProductColors)
                .Where(c => c.IsActive)
                .Select(c => c.Color).Distinct()
                .Select(c => new Tuple<int, string, string, string>(
                    c.ColorId,
                    c.ColorName,
                    c.ColorCode,
                    _context.Products.Where(p => p.ProductId == productId && p.IsActive)
                        .SelectMany(p => p.ShopperProducts)
                        .Where(co => co.IsActive)
                        .SelectMany(a => a.ShopperProductColors)
                        .Where(co => co.IsActive && co.ColorId == c.ColorId)
                        .OrderByDescending(o => o.SaleCount)
                        .First().ShopperProductColorId
                ));

        public IEnumerable<Tuple<int, string>> GetProductColors(int productId) =>
            _context.ShopperProducts.Where(c => c.ProductId == productId)
                .SelectMany(c => c.ShopperProductColors)
                .Select(c => c.Color).Distinct()
                .Select(c => new Tuple<int, string>(c.ColorId, c.ColorName));

        public async Task<Tuple<int, string>> GetColorByIdAsync(int colorId) =>
            await _context.Colors.Where(c => c.ColorId == colorId)
                .Select(c => new Tuple<int, string>(c.ColorId, c.ColorName))
                .SingleOrDefaultAsync();

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId) =>
            _context.OrderDetails.Where(c =>
                    c.ShopperProductColor.ShopperProduct.ProductId == productId &&
                    c.ShopperProductColor.ColorId == colorId &&
                    c.Order.IsPayed &&
                    c.Order.PayDate >= DateTime.Now.AddDays(-30))
                .OrderBy(c => c.Order.PayDate)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Order.PayDate.Value.ToShamsiDate(),
                    ViewCount = 10,
                    SellCount = c.Count,
                });

        public IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId) =>
            _context.Products.Where(c => c.ProductId == productId)
                .SelectMany(c => c.ShopperProducts)
                .SelectMany(c => c.ShopperProductColors)
                .Select(c => new Tuple<string, int, int, int>(
                    c.Color.ColorName,
                    10,
                    _context.OrderDetails
                        .Where(o => o.ShopperProductColorId == c.ShopperProductColorId)
                        .Sum(s => s.Count), 10));
    }
}