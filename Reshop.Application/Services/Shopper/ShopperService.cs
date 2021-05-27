using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
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


        public async Task<string> AddShopperAsync(Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                await _shopperRepository.AddShopperAsync(shopper);
                await _shopperRepository.SaveChangesAsync();

                return shopper.ShopperId;
            }
            catch
            {
                return null;
            }
        }
    }
}
