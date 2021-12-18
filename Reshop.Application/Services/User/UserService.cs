using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Security;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Domain.Interfaces.Shopper;

namespace Reshop.Application.Services.User
{
    public class UserService : IUserService
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _roleRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IShopperRepository _shopperRepository;

        public UserService(IUserRepository userRepository, IPermissionRepository roleRepository, IQuestionRepository questionRepository, IShopperRepository shopperRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _questionRepository = questionRepository;
            _shopperRepository = shopperRepository;
        }

        #endregion

        public async Task<AddOrEditUserForAdminViewModel> GetUserDataForAdminAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = await _roleRepository.GetRolesAsync();
            var userRoles = await _roleRepository.GetRolesIdOfUserAsync(userId);

            return new AddOrEditUserForAdminViewModel()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Score = user.Score,
                AccountBalance = user.AccountBalance,
                NationalCode = user.NationalCode,
                IsBlocked = user.IsBlocked,
                Roles = roles,
                SelectedRoles = userRoles
            };
        }

        public async Task<EditUserViewModel> GetUserDataForEditAsync(string userId) =>
            await _userRepository.GetUserDataForEditAsync(userId);

        public async Task<UserDetailViewModel> GetUserDetailAsync(string userId)
        {
            var user = await _userRepository.GetUserDetailAsync(userId);

            var shopperId = await _shopperRepository.GetShopperIdOfUserByUserId(user.UserId);

            user.ShopperId = shopperId;

            return user;
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

        public IEnumerable<UserInformationForListViewModel> GetUsersInformation()
            =>
                _userRepository.GetUsersInformation();

        public IEnumerable<Address> GetUserAddresses(string userId)
            =>
                _userRepository.GetUserAddresses(userId);

        public async Task<IEnumerable<AddressForShowViewModel>> GetUserAddressesForShowAsync(string userId) =>
            await _userRepository.GetUserAddressesForShowAsync(userId);

        public async Task<bool> IsPhoneNumberExistAsync(string phoneNumber)
        {
            return await _userRepository.IsPhoneNumberExistAsync(phoneNumber);
        }

        public async Task<bool> IsUserPasswordValidAsync(string userId, string password)
        {
            string encodedPass = PasswordHelper.EncodePasswordMd5(password);

            return await _userRepository.IsUserPasswordValidAsync(userId, encodedPass);
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
                if (userRoles != null)
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
            _questionRepository.GetUserQuestionsForShow(userId);

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

        public async Task<ResultTypes> EditUserAddressAsync(Address address)
        {
            try
            {
                _userRepository.UpdateAddress(address);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteUserAddressAsync(string addressId)
        {
            try
            {
                var address = await _userRepository.GetAddressByIdAsync(addressId);

                if (address == null)
                    return ResultTypes.Failed;

                _userRepository.RemoveAddress(address);

                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteUserAddressAsync(Address address)
        {
            try
            {
                _userRepository.RemoveAddress(address);

                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Address> GetAddressByIdAsync(string addressId) =>
            await _userRepository.GetAddressByIdAsync(addressId);

        public async Task<bool> IsUserAddressExistAsync(string addressId, string userId) =>
            await _userRepository.IsUserAddressExistAsync(addressId, userId);
    }
}
