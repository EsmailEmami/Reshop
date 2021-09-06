using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Chart;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Interfaces.Shopper
{
    public interface IShopperRepository
    {
        Task<bool> IsShopperExistAsync(string shopperId);
        Task AddShopperAsync(Entities.Shopper.Shopper shopper);
        void EditShopper(Entities.Shopper.Shopper shopper);
        Task<EditShopperViewModel> GetShopperDataForEditAsync(string shopperId);
        Task<string> GetShopperIdOfUserByUserId(string userId);

        Task<string> GetShopperProductIdAsync(string shopperId, int productId);

        // types = all,active,existed
        IEnumerable<ShoppersListForAdmin> GetShoppersWithPagination(string type = "all", int skip = 0, int take = 18, string filter = null);
        IEnumerable<ShoppersListForAdmin> GetProductShoppersWithPagination(int productId, string type = "all", int skip = 0, int take = 18, string filter = null);
        IEnumerable<ShopperProductsListForShow> GetShopperProductsWithPagination(string shopperId, string type = "all", int skip = 0, int take = 18, string filter = null);
        Task<int> GetShoppersCountWithTypeAsync(string type = "all");
        Task<int> GetShopperProductsCountWithTypeAsync(string shopperId, string type = "all");
        Task<ShopperDataForAdmin> GetShopperDataForAdminAsync(string shopperId);

        #region reason

        Task AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest);
        Task AddShopperProductColorRequestAsync(ShopperProductColorRequest shopperProductColorRequest);
        IEnumerable<ShopperRequestsForShowViewModel> GetShopperProductColorRequestsForShow(string shopperId, int skip, int take);
        IEnumerable<ShopperRequestsForShowViewModel> GetShopperProductRequestsForShow(string shopperId, int skip, int take);
        Task<int> GetShopperProductColorRequestsCountAsync(string shopperId);
        Task<int> GetShopperProductRequestsCountAsync(string shopperId);

        #endregion


        #region shopper product


        void UpdateShopperProduct(ShopperProduct shopperProduct);
        void RemoveShopperProduct(ShopperProduct shopperProduct);
        Task AddShopperProductAsync(ShopperProduct shopperProduct);
        IEnumerable<ShopperProduct> GetShoppersOfProduct(int productId);
        Task<bool> IsShopperProductExistAsync(string shopperId, int productId);
        Task<bool> IsShopperProductExistAsync(string shopperProductId);

        #endregion


        #region store title

        IEnumerable<StoreTitle> GetStoreTitles();
        Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId);
        Task AddStoreTitleAsync(StoreTitle storeTitle);
        void UpdateStoreTitle(StoreTitle storeTitle);
        void RemoveStoreTitle(StoreTitle storeTitle);

        Task AddShopperStoreTitleAsync(ShopperStoreTitle shopperStoreTitle);
        void RemoveShopperStoreTitle(ShopperStoreTitle shopperStoreTitle);

        IEnumerable<string> GetShopperStoreTitlesName(string shopperId);
        IEnumerable<Tuple<int, string>> GetShopperStoreTitles(string shopperId);

        #endregion

        #region address

        IEnumerable<StoreAddress> GetShopperStoreAddresses(string shopperId);
        Task AddStoreAddressAsync(StoreAddress storeAddress);
        void EditStoreAddress(StoreAddress storeAddress);
        void RemoveStoreAddress(StoreAddress storeAddress);
        Task<StoreAddress> GetStoreAddressByIdAsync(string storeAddressId);

        #endregion

        #region Discount

        Task<ShopperProductDiscount> GetLastShopperProductDiscountAsync(string shopperProductColorId);
        Task AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        Task<bool> IsActiveShopperProductColorDiscountExistsAsync(string shopperProductColorId);

        #endregion

        #region color

        IEnumerable<Color> GetColors();
        Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductColorId);
        Task<ShopperProductColor> GetShopperProductColorAsync(string shopperProductId, int colorId);
        Task AddShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductId, string shopperProductColorId);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductColorId);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductId, int colorId);
        void UpdateShopperProductColor(ShopperProductColor shopperProductColor);
        Task<string> GetShopperProductColorIdAsync(string shopperProductId, int colorId);
        Task<ShopperProductColorDetailViewModel> GetShopperProductColorDetailAsync(string shopperProductColorId);
        Task<ShopperProductColorDiscountDetailViewModel> GetShopperProductColorDiscountDetailAsync(string shopperProductColorId);
        Task<bool> IsAnyActiveShopperProductColorRequestAsync(string shopperProductId, int colorId);

        #endregion

        #region Chart

        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayProductDataChart(string shopperProductId);
        IEnumerable<LastThirtyDayProductDataChart> GetLastThirtyDayColorProductDataChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetLastThirtyDayBestShoppersOfColorProductChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfProductChart(int productId);
        IEnumerable<Tuple<string, int>> GetBestShoppersOfColorProductChart(string shopperProductColorId);
        IEnumerable<Tuple<string, int>> GetLastTwentyDiscountDataOfShopperProductColorChart(string shopperProductColorId);
        // colorName , view , sell , returned
        IEnumerable<Tuple<string, int, int, int>> GetColorsOfShopperProductDataChart(string shopperProductId);
        #endregion

        Task SaveChangesAsync();
    }
}
