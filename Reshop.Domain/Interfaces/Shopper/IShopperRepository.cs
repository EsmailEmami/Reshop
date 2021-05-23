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

        #region shopper product

        Task<ShopperProduct> GetShopperProductAsync(string shopperUserId, int productId);
        void UpdateShopperProduct(ShopperProduct shopperProduct);

        #endregion


        #region store title

        Task AddStoreTitleAsync(StoreTitle storeTitle);
        void UpdateStoreTitle(StoreTitle storeTitle);
        void RemoveStoreTitle(StoreTitle storeTitle);

        Task AddShopperStoreTitleAsync(ShopperStoreTitle shopperStoreTitle);
        void RemoveShopperStoreTitle(ShopperStoreTitle shopperStoreTitle);


        #endregion


        Task SaveChangesAsync();
    }
}
