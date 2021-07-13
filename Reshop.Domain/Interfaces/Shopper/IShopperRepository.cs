using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Shopper
{
    public interface IShopperRepository
    {
        Task<Entities.User.User> GetShopperByIdAsync(string userId);
        Task<bool> IsShopperExistAsync(string shopperId);
        Task AddShopperAsync(Entities.Shopper.Shopper shopper);
        void EditShopper(Entities.Shopper.Shopper shopper);
        Task<EditShopperViewModel> GetShopperDataForEditAsync(string userId);
        Task<string> GetShopperIdOfUserByUserId(string userId);

        // types = all,active,block
        IEnumerable<ShoppersListForAdmin> GetShoppersWithPagination(string type = "all", int skip = 0, int take = 18, string filter = null);

        #region reason

        Task AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest);

        #endregion


        #region shopper product


        void UpdateShopperProduct(ShopperProduct shopperProduct);
        void RemoveShopperProduct(ShopperProduct shopperProduct);
        Task AddShopperProductAsync(ShopperProduct shopperProduct);
        IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId);
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
        #endregion

        #region address

        Task AddStoreAddressAsync(StoreAddress storeAddress);
        void EditStoreAddress(StoreAddress storeAddress);
        void RemoveStoreAddress(StoreAddress storeAddress);
        Task<StoreAddress> GetStoreAddressByIdAsync(string storeAddressId);

        #endregion

        #region Discount

        Task<ShopperProductDiscount> GetLastShopperProductDiscountAsync(string shopperProductId);

        Task AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        #endregion



        Task SaveChangesAsync();
    }
}
