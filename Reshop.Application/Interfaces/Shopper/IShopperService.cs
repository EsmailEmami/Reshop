using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Application.Interfaces.Shopper
{
    public interface IShopperService
    {
        Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper);
        Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct);



        #region store title

        IEnumerable<StoreTitle> GetStoreTitles();
        Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId);
        Task<ResultTypes> AddStoreTitleAsync(StoreTitle storeTitle);
        Task<ResultTypes> EditStoreTitleAsync(StoreTitle storeTitle);
        Task<ResultTypes> DeleteStoreTitleAsync(int storeTitleId);

        #endregion
    }
}