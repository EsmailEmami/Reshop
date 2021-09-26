using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Product;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
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

        public IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId,
            int colorId) => _colorRepository.GetLastThirtyDayColorProductDataChart(productId, colorId);

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

        public IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId) =>
            _colorRepository.GetColorsOfProductDataChart(productId);
    }
}