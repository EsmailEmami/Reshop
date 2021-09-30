using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Product;

namespace Reshop.Application.Interfaces.Product
{
    public interface IColorService
    {
        Task<Tuple<IEnumerable<Color>, int, int>> GetColorsWithPaginationAsync(int pageId = 1, int take = 25, string filter = "");
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(int productId, int colorId);

        Task<ProductColorDetailViewModel> GetProductColorDetailAsync(int productId, int colorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfProductDataChart(int productId);

        Task<Color> GetColorByIdAsync(int colorId);
        Task<ResultTypes> AddColorAsync(Color color);
        Task<ResultTypes> EditColorAsync(Color color);
    }
}