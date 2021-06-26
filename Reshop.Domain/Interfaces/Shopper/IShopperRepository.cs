using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.Interfaces.Shopper
{
    public interface IShopperRepository
    {
        Task<Entities.User.User> GetShopperByIdAsync(string userId);
        Task AddShopperAsync(Entities.Shopper.Shopper shopper);
        Task<AddOrEditShopperViewModel> GetShopperDataForEditAsync(string userId);

        // types = all,active,block
        IEnumerable<ShoppersListForAdmin> GetShoppersWithPagination(string type = "all", int skip = 0, int take = 18, string filter = null);


        #region shopper product


        void UpdateShopperProduct(ShopperProduct shopperProduct);
        void RemoveShopperProduct(ShopperProduct shopperProduct);
        Task AddShopperProductAsync(ShopperProduct shopperProduct);
        IEnumerable<Tuple<string, string, string>> GetProductShoppers(int productId);
        IEnumerable<ShopperProduct> GetProductShoppersProduct(int productId);
        Task<bool> IsShopperProductExistAsync(string shopperUserId, int productId);

        #endregion


        #region store title

        IEnumerable<StoreTitle> GetStoreTitles();
        Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId);
        Task AddStoreTitleAsync(StoreTitle storeTitle);
        void UpdateStoreTitle(StoreTitle storeTitle);
        void RemoveStoreTitle(StoreTitle storeTitle);

        Task AddShopperStoreTitleAsync(ShopperStoreTitle shopperStoreTitle);
        void RemoveShopperStoreTitle(ShopperStoreTitle shopperStoreTitle);


        #endregion


        Task SaveChangesAsync();
    }
}
