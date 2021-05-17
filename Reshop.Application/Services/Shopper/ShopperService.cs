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


        public async Task<Domain.Entities.User.User> AddShopperAsync(Domain.Entities.User.User user, Domain.Entities.Shopper.Shopper shopper)
        {
            try
            {
                await _shopperRepository.AddShopperAsync(shopper);
                await _shopperRepository.SaveChangesAsync();

                user.ShopperId = shopper.ShopperId;
                user.Shopper = shopper;
                user.IsUserShopper = true;

                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
