using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Interfaces.Product;
using Reshop.Domain.Interfaces.User;

namespace Reshop.Application.Services.Product
{
    public class ColorService : IColorService
    {
        #region constructor

        private readonly IColorRepository _colorRepository;
        private readonly ICartRepository _cartRepository;

        public ColorService(IColorRepository colorRepository, ICartRepository cartRepository)
        {
            _colorRepository = colorRepository;
            _cartRepository = cartRepository;
        }

        #endregion

        public async Task<Tuple<IEnumerable<Color>, int, int>> GetColorsWithPaginationAsync(int pageId = 1, int take = 25, string filter = "")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int count = await _colorRepository.GetColorsCountAsync();

            var data = _colorRepository.GetColorsWithPagination(skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * count / take);


            return new Tuple<IEnumerable<Color>, int, int>(data, pageId, totalPages);
        }

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId)
        {
            var data = _colorRepository.GetLastThirtyDayColorProductDataChart(productId, colorId);

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

        public async Task<ProductColorDetailViewModel> GetProductColorDetailAsync(int productId, int colorId)
        {
            var color = await _colorRepository.GetColorByIdAsync(colorId);
            int lastMonthSellCount = await _cartRepository.GetSellCountOfProductColorFromDateAsync(productId, colorId, DateTime.Now.AddDays(-30));
            int sellCount = await _cartRepository.GetSellCountOfProductColorFromDateAsync(productId, colorId);

            return new ProductColorDetailViewModel()
            {
                ProductId = productId,
                ColorId = color.Item1,
                ColorName = color.Item2,
                LastMonthSellCount = lastMonthSellCount,
                SellCount = sellCount,
                ReturnedCount = 10,
            };
        }

        public IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId)
        {
            var data = _colorRepository.GetColorsOfProductDataChart(productId);

            if (data == null)
                return null;

            var finalData = data.GroupBy(c => c.Item1)
            .Select(c => new Tuple<string, int, int, int>(
                c.Key,
                10,
                c.Sum(g => g.Item3),
                5))
            .ToList();

            if (!finalData.Any())
                return null;

            return finalData;
        }

        public async Task<Color> GetColorByIdAsync(int colorId) =>
            await _colorRepository.GetRealColorByIdAsync(colorId);

        public async Task<ResultTypes> AddColorAsync(Color color)
        {
            try
            {
                await _colorRepository.AddColorAsync(color);
                await _colorRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditColorAsync(Color color)
        {
            try
            {
                _colorRepository.UpdateColor(color);
                await _colorRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }
    }
}