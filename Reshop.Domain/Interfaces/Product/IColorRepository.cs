using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IColorRepository
    {
        IEnumerable<Tuple<int, string, string, string>> GetProductColorsWithDetail(int productId);
        IEnumerable<Tuple<int, string>> GetProductColors(int productId);
        Task<Tuple<int, string>> GetColorByIdAsync(int colorId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);
    }
}