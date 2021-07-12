using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct);
        Task<bool> IsShopperExistAsync(string shopperId);

        #region address

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

        IEnumerable<string> GetShopperStoreTitlesName(string shopperUserId);

        #endregion
    }
}