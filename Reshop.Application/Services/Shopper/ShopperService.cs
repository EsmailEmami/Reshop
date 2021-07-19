using System;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Domain.DTOs.Shopper;

namespace Reshop.Application.Services.Shopper
{
    public class ShopperService : IShopperService
    {
        #region Constructor

        private readonly IShopperRepository _shopperRepository;
        private readonly IUserRepository _userRepository;

        public ShopperService(IShopperRepository shopperRepository, IUserRepository userRepository)
        {
            _shopperRepository = shopperRepository;
            _userRepository = userRepository;
        }

        #endregion


        public async Task<Tuple<IEnumerable<ShoppersListForAdmin>, int, int>> GetShoppersInformationWithPagination(string type = "all",string filter = "", int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int shoppersCount = await _shopperRepository.GetShoppersCountWithTypeAsync(type.FixedText());

            var shoppers = _shopperRepository.GetShoppersWithPagination(type.FixedText(),skip,take,filter);

            int totalPages = (int)Math.Ceiling(1.0 * shoppersCount / take);


            return new Tuple<IEnumerable<ShoppersListForAdmin>, int, int>(shoppers, pageId, totalPages);
        }

        public async Task<ResultTypes> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                await _shopperRepository.AddShopperAsync(shopper);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperAsync(Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                _shopperRepository.EditShopper(shopper);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperProductAsync(ShopperProduct shopperProduct)
        {
            try
            {
                await _shopperRepository.AddShopperProductAsync(shopperProduct);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditShopperProductAsync(ShopperProduct shopperProduct)
        {
            try
            {
                _shopperRepository.UpdateShopperProduct(shopperProduct);
                await _shopperRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperProductDiscountAsync(ShopperProductDiscount shopperProductDiscount)
        {
            try
            {
                await _shopperRepository.AddShopperProductDiscountAsync(shopperProductDiscount);
                await _shopperRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsShopperExistAsync(string shopperId) =>
            await _shopperRepository.IsShopperExistAsync(shopperId);

        public async Task<string> GetShopperIdOrUserAsync(string userId)
        {
            try
            {
                return await _shopperRepository.GetShopperIdOfUserByUserId(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ResultTypes> AddShopperProductRequestAsync(ShopperProductRequest shopperProductRequest)
        {
            try
            {
                await _shopperRepository.AddShopperProductRequestAsync(shopperProductRequest);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<IEnumerable<StoreAddress>> GetShopperStoreAddressesAsync(string shopperUserId)
        {
            var shopperId =await _shopperRepository.GetShopperIdOfUserByUserId(shopperUserId);

            return _shopperRepository.GetShopperStoreAddresses(shopperId);
        }

        public async Task<ResultTypes> AddStoreAddressAsync(StoreAddress storeAddress)
        {
            try
            {
                await _shopperRepository.AddStoreAddressAsync(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditStoreAddressAsync(StoreAddress storeAddress)
        {
            try
            {
                _shopperRepository.EditStoreAddress(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveStoreAddressAsync(string storeAddressId)
        {
            var storeAddress = await _shopperRepository.GetStoreAddressByIdAsync(storeAddressId);

            if (storeAddress is null) return ResultTypes.Failed;

            try
            {
                _shopperRepository.RemoveStoreAddress(storeAddress);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<StoreTitle> GetStoreTitles() => _shopperRepository.GetStoreTitles();

        public async Task<StoreTitle> GetStoreTitleByIdAsync(int storeTitleId) =>
            await _shopperRepository.GetStoreTitleByIdAsync(storeTitleId);

        public async Task<ResultTypes> AddStoreTitleAsync(StoreTitle storeTitle)
        {
            try
            {
                await _shopperRepository.AddStoreTitleAsync(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditStoreTitleAsync(StoreTitle storeTitle)
        {
            try
            {
                _shopperRepository.UpdateStoreTitle(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteStoreTitleAsync(int storeTitleId)
        {
            var storeTitle = await _shopperRepository.GetStoreTitleByIdAsync(storeTitleId);

            if (storeTitle is null) return ResultTypes.Failed;

            try
            {
                _shopperRepository.RemoveStoreTitle(storeTitle);
                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddShopperStoreTitleAsync(string shopperId, List<int> storeTitlesId)
        {
            try
            {
                foreach (var storeTitleId in storeTitlesId)
                {
                    await _shopperRepository.AddShopperStoreTitleAsync(new ShopperStoreTitle()
                    {
                        ShopperId = shopperId,
                        StoreTitleId = storeTitleId
                    });
                }

                await _shopperRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<string> GetShopperStoreTitlesName(string shopperId)
        {
            return _shopperRepository.GetShopperStoreTitlesName(shopperId);
        }

        public async Task<bool> IsShopperProductColorExistAsync(string shopperProductId, string shopperProductColorId) =>
            await _shopperRepository.IsShopperProductColorExistAsync(shopperProductId, shopperProductColorId);
    }
}
