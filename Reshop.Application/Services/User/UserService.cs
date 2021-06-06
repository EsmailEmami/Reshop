using Reshop.Application.Interfaces.User;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Interfaces.Shopper;

namespace Reshop.Application.Services.User
{
    public class UserService : IUserService
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IShopperRepository _shopperRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IShopperRepository shopperRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _shopperRepository = shopperRepository;
        }

        #endregion

        public async Task<AddOrEditUserViewModel> GetUserDataAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = _roleRepository.GetRoles();
            var userRoles = _roleRepository.GetRolesIdOfUser(userId);

            return new AddOrEditUserViewModel()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Score = user.Score,
                AccountBalance = user.AccountBalance,
                NationalCode = user.NationalCode,
                PostalCode = user.PostalCode,
                IsPhoneNumberActive = user.IsPhoneNumberActive,
                IsBlocked = user.IsBlocked,
                Roles = roles as IEnumerable<Role>,
                SelectedRoles = userRoles as IEnumerable<string>
            };
        }

        public int GetUserWalletBalance(string userId)
        {

            if (userId == null) return 0;

            int deposit = (int)_userRepository.GetDepositUserWallet(userId).Sum();

            int withdraw = (int)_userRepository.GetWithdrawUserWallet(userId).Sum();

            return (deposit - withdraw);
        }

        public IAsyncEnumerable<Domain.Entities.User.User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public IAsyncEnumerable<UserInformationViewModel> GetUsersInformation()
            =>
                _userRepository.GetUsersInformation();


        //public async Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18)
        //{
        //    int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

        //    int productsCount = await _shopperRepository.GetShoppersCountAsync();

        //    var shoppers = _userRepository.GetShoppersWithPagination(skip, take);

        //    int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

        //    return new Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>(shoppers, pageId, totalPages);
        //}

        public IAsyncEnumerable<Address> GetUserAddresses(string userId)
            =>
                _userRepository.GetUserAddresses(userId);

        public async Task<bool> IsPhoneExistAsync(string phone)
        {
            return await _userRepository.IsPhoneExistAsync(phone);
        }

        public async Task<bool> IsUserExistAsync(string userId)
        {
            return await _userRepository.IsUserExistAsync(userId);
        }


        public async Task<ResultTypes> AddUserAsync(Domain.Entities.User.User user)
        {
            try
            {
                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Domain.Entities.User.User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                return await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ResultTypes> EditUserAsync(Domain.Entities.User.User user)
        {
            try
            {
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task AddUserToInvitesAsync(string inviteCode, string invitedUserId)
        {
            // caller
            var user = await _userRepository.GetUserByInviteCodeAsync(inviteCode);


            if (user != null)
            {
                user.InviteCount++;
                user.Score += 10;
                _userRepository.UpdateUser(user);

                var userInvite = new UserInvite()
                {
                    InviterUserId = user.UserId,
                    InvitedUserId = invitedUserId
                };
                await _userRepository.AddUserInviteAsync(userInvite);

                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task<Domain.Entities.User.User> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<ResultTypes> RemoveUserAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null) return ResultTypes.Failed;

            var userRoles = _roleRepository.GetUserRolesByUserId(user.UserId);
            try
            {
                if (userRoles is not null)
                {
                    await foreach (var userRole in userRoles)
                    {
                        _roleRepository.RemoveUserRole(userRole);
                    }

                    await _roleRepository.SaveChangesAsync();
                }

                _userRepository.RemoveUser(user);
                await _userRepository.SaveChangesAsync();


                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId) =>
            _userRepository.GetUserQuestionsForShow(userId);

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId) => 
            _userRepository.GetUserCommentsForShow(userId);

        public async Task<ResultTypes> AddUserAddressAsync(Address address)
        {
            try
            {
                await _userRepository.AddAddressAsync(address);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }
    }
}
