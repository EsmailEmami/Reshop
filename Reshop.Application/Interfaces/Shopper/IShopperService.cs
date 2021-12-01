using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        // data , pageId , totalPages
        Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int>> GetShoppersInformationWithPagination(string type = "all", string filter = "", int pageId = 1, int take = 18);
        Task<Tuple<IEnumerable<ShopperProductsListForShow>, int, int>> GetShopperProductsInformationWithPagination(string shopperId, string type = "all", string filter = "", int pageId = 1, int take = 18);
        Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int, int>> GetProductShoppersInformationWithPagination(int productId, string type = "all", string filter = "", int pageId = 1, int take = 18);

        Task<bool> IsShopperProductColorOfShopperAsync(string shopperId, string shopperProductColorId);
        Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct);
        Task<ResultTypes> EditShopperProductAsync(ShopperProduct shopperProduct);
        Task<bool> IsShopperExistAsync(string shopperId);
        Task<bool> IsUserShopperAsync(string userId);
        Task<Domain.Entities.Shopper.Shopper> GetShopperByIdAsync(string shopperId);
        Task<ResultTypes> DeleteShopperAsync(string shopperId);

        Task<bool> IsShopperProductOfShopperAsync(string shopperId, string shopperProductId);
        Task<ResultTypes> UnAvailableShopperProductAsync(string shopperId, int productId);
        Task<string> GetShopperIdOrUserAsync(string userId);
        Task<string> GetShopperProductIdAsync(string shopperId, int productId);

        Task<ShopperDataForAdmin> GetShopperDataForAdminAsync(string shopperId);

        Task<EditShopperProductViewModel> GetShopperProductDataForEditAsync(string shopperProductId);

        #region reason

        // type = add , edit
        Task<bool> IsAnyActiveShopperProductRequestExistAsync(string shopperId, int productId, bool type);
        Task<bool> IsAnyActiveShopperProductRequestExistAsync(string shopperProductId, bool type);
        Task<ResultTypes> AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest);
        Task<ResultTypes> AddShopperProductColorRequestAsync(ShopperProductColorRequest shopperProductColorRequest);
        // data , pageId , totalPages
        Task<Tuple<IEnumerable<ShopperRequestsForShowViewModel>, int, int>> GetShopperRequestsForShowAsync(string shopperId, string type = "all", int pageId = 1, int take = 18, string filter = null);
        Task<ShopperProductRequest> GetShopperProductRequestAsync(string shopperProductRequestId);
        Task<ShopperProductColorRequest> GetShopperProductColorRequestAsync(string shopperProductColorRequestId);

        Task<ShopperProductRequestForShowViewModel> GetShopperProductRequestForShowAsync(string shopperProductRequestId);
        Task<ShopperProductColorRequestForShowViewModel> GetShopperProductColorRequestForShowAsync(string shopperProductColorRequestId);
        Task<ShopperProductRequestForShowShopperViewModel> GetShopperProductRequestForShowShopperAsync(string shopperProductRequestId);
        Task<ShopperProductColorRequestForShowShopperViewModel> GetShopperProductColorRequestForShowShopperAsync(string shopperProductColorRequestId);
        Task<ResultTypes> FinishShopperProductRequestAsync(FinishShopperRequestViewModel model);
        Task<ResultTypes> FinishShopperProductColorRequestAsync(FinishShopperRequestViewModel model);

        #endregion


        #region address

        Task<IEnumerable<StoreAddressForShowViewModel>> GetShopperStoreAddressesForShowAsync(string shopperId);
        Task<ResultTypes> AddStoreAddressAsync(StoreAddress storeAddress);
        Task<ResultTypes> EditStoreAddressAsync(StoreAddress storeAddress);
        Task<ResultTypes> DeleteStoreAddressAsync(string storeAddressId);
        Task<StoreAddress> GetStoreAddressByIdAsync(string storeAddressId);

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
        IEnumerable<Tuple<int, string>> GetColorsIdAndName();
        Task<string> GetShopperProductColorIdAsync(string shopperId, int productId, int colorId);
        Task<string> GetShopperProductColorIdAsync(string shopperProductId, int colorId);
        Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductColorId);
        Task<ResultTypes> AddShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<ResultTypes> EditShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductId, int colorId);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductColorId);

        Task<ShopperProductColorDetailViewModel> GetShopperProductColorDetailAsync(string shopperProductColorId);
        Task<bool> IsAnyActiveShopperProductColorRequestExistAsync(string shopperProductId, int colorId, bool type);

        #endregion

        #region chart

        Task<IEnumerable<LastThirtyDayProductDataChart>> GetLastThirtyDayProductDataChartAsync(int productId, string shopperId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfColorProductChart(int productId, int colorId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfColorProductChart(int productId, int colorId);
        // colorName , view , sell , returned
        Task<IEnumerable<Tuple<string, int, int, int>>> GetColorsOfShopperProductDataChartAsync(int productId, string shopperId);

        #endregion
    }
}