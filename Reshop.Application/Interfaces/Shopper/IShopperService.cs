using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        // data , pageId , totalPages
        Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int>> GetShoppersInformationWithPagination(string type = "all", string filter = "", int pageId = 1, int take = 18);


        Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct);
        Task<ResultTypes> EditShopperProductAsync(ShopperProduct shopperProduct);
        Task<ResultTypes> AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount);
        Task<bool> IsShopperExistAsync(string shopperId);

        Task<string> GetShopperIdOrUserAsync(string userId);

        #region reason

        Task<ResultTypes> AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest);

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

        #endregion

        #region color

        Task<ResultTypes> AddShopperProductColorAsync(ShopperProductColor shopperProductColor);
        Task<bool> IsShopperProductColorExistAsync(string shopperProductId, string shopperProductColorId);

        #endregion
    }
}