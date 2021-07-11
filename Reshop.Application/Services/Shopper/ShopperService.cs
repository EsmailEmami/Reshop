using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Interfaces.Shopper;
using Reshop.Domain.Interfaces.User;

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

        public async Task<bool> IsShopperExistAsync(string shopperUserId) =>
            await _shopperRepository.IsShopperExistAsync(shopperUserId);

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

        public IEnumerable<string> GetShopperStoreTitlesName(string shopperUserId)
        {
            string shopperId = _shopperRepository.GetShopperIdOfUserByUserId(shopperUserId);
            return _shopperRepository.GetShopperStoreTitlesName(shopperId);
        }
    }
}
