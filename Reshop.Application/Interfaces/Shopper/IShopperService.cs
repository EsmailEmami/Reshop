using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        // data , pageId , totalPages
        Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int>> GetShoppersInformationWithPagination(string type = "all", string filter = "", int pageId = 1, int take = 18);
        Task<Tuple<IEnumerable<ShopperProductsListForShow>, int, int>> GetShopperProductsInformationWithPagination(string shopperId, string type = "all", string filter = "", int pageId = 1, int take = 18);
        Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int, int>> GetProductShoppersInformationWithPagination(int productId, string type = "all", string filter = "", int pageId = 1, int take = 18);

        Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct);
        Task<ResultTypes> EditShopperProductAsync(ShopperProduct shopperProduct);
        Task<bool> IsShopperExistAsync(string shopperId);

        Task<ResultTypes> UnAvailableShopperProductAsync(string shopperId, int productId);
        Task<string> GetShopperIdOrUserAsync(string userId);
        Task<string> GetShopperProductIdAsync(string shopperId, int productId);

        Task<ShopperDataForAdmin> GetShopperDataForAdminAsync(string shopperId);
        #region reason

        Task<ResultTypes> AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest);
        Task<ResultTypes> AddShopperProductColorRequestAsync(ShopperProductColorRequest shopperProductColorRequest);
        // data , pageId , totalPages
        Task<Tuple<IEnumerable<ShopperRequestsForShowViewModel>, int, int>> GetShopperRequestsForShowAsync(string shopperId, int pageId = 1, int take = 18);


        #endregion

        #region discount

        Task<ResultTypes> AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        Task<ShopperProductDiscount> GetLastShopperProductColorDiscountAsync(string shopperProductColorId);
        Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId);

        #endregion

        #region address

        Task<IEnumerable<StoreAddress>> GetShopperStoreAddressesAsync(string shopperUserId);
        Task<ResultTypes> AddStoreAddressAsync(StoreAddress storeAddress);
        Task<ResultTypes> EditStoreAddressAsync(StoreAddress storeAddress);
        Task<ResultTypes> RemoveStoreAddressAsync(string storeAddressId);

        #endregion

        #region store title

        IEnumerable<StoreTitle> GetStoreTitles();
        Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId);
        Task<ResultTypes> AddStoreTitleAsync(StoreTitle storeTitle);
        Task<ResultTypes> EditStoreTitleAsync(StoreTitle storeTitle);
        Task<ResultTypes> DeleteStoreTitleAsync(int storeTitleId);
        Task<ResultTypes> AddShopperStoreTitleAsync(string shopperId, List<int> storeTitlesId);

        IEnumerable<string> GetShopperStoreTitlesName(string shopperId);
        IEnumerable<Tuple<int, string>> GetShopperStoreTitles(string shopperId);

        #endregion

        #region color

        IEnumerable<Color> GetColors();
        Task<string> GetShopperProductColorIdAsync(string shopperId, int productId, int colorId);
        Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductColorId);
        Task<ResultTypes> AddShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<ResultTypes> EditShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductId, string shopperProductColorId);
        Task<ShopperProductColorDetailViewModel> GetShopperProductColorDetailAsync(string shopperProductColorId);
        Task<ShopperProductColorDiscountDetailViewModel> GetShopperProductColorDiscountDetailAsync(string shopperProductColorId);
        Task<bool> IsAnyActiveShopperProductColorRequestAsync(string shopperProductId, int colorId);

        #endregion

        #region chart

        Task<IEnumerable<LastThirtyDayProductDataChart>> GetLastThirtyDayProductDataChartAsync(int productId, string shopperId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfColorProductChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfColorProductChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId);
        // colorName , view , sell , returned
        Task<IEnumerable<Tuple<string, int, int, int>>> GetColorsOfShopperProductDataChartAsync(int productId, string shopperId);

        #endregion
    }
}