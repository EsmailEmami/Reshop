using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Interfaces.Product
{
    public interface IColorRepository
    {
        IEnumerable<Tuple<int, string, string, string>> GetProductColorsWithDetail(int productId);
        IEnumerable<Tuple<int, string>> GetProductColors(int productId);
        Task<Tuple<int, string>> GetColorByIdAsync(int colorId);
        Task<Color> GetRealColorByIdAsync(int colorId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);
        Task<int> GetColorsCountAsync();
        IEnumerable<Color> GetColorsWithPagination(int skip, int take, string filter);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);

        Task AddColorAsync(Color color);
        void UpdateColor(Color color);


        Task SaveChangesAsync();
    }
}