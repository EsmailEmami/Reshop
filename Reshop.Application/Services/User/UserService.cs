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
using Reshop.Domain.Interfaces.Conversation;

namespace Reshop.Application.Services.User
{
    public class UserService : IUserService
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _roleRepository;
        private readonly IQuestionRepository _questionRepository;

        public UserService(IUserRepository userRepository, IPermissionRepository roleRepository, IQuestionRepository questionRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _questionRepository = questionRepository;
        }

        #endregion

        public async Task<AddOrEditUserForAdminViewModel> GetUserDataForAdminAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles =await _roleRepository.GetRolesAsync();
            var userRoles =await _roleRepository.GetRolesIdOfUserAsync(userId);

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

        public async Task<UserDetailViewModel> GetUserDetailAsync(string userId) =>
            await _userRepository.GetUserDetailAsync(userId);

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


        //public async Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18)
        //{
        //    int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

        //    int productsCount = await _shopperRepository.GetShoppersCountAsync();

        //    var shoppers = _userRepository.GetShoppersWithPagination(skip, take);

        //    int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

        //    return new Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>(shoppers, pageId, totalPages);
        //}

        public IEnumerable<Address> GetUserAddresses(string userId)
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

        public async Task<Address> GetAddressByIdAsync(string addressId) =>
            await _userRepository.GetAddressByIdAsync(addressId);

        public async Task<bool> IsUserAddressExistAsync(string addressId, string userId) =>
            await _userRepository.IsUserAddressExistAsync(addressId, userId);
    }
}
