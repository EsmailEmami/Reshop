using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;

namespace Reshop.Application.Interfaces.Product
{
    public interface IColorService
    {
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);

        Task<ProductColorDetailViewModel> GetProductColorDetailAsync(int productId, int colorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);
    }
}