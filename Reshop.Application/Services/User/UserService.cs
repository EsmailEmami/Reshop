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
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Product;

namespace Reshop.Application.Services.User
{
    public class UserService : IUserService
    {
        #region constructor

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        public async Task<AddOrEditUserViewModel> GetUserDataAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = _userRepository.GetRoles();
            var userRoles = _userRepository.GetRolesIdOfUser(userId);

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
                Roles = roles,
                SelectedRoles = userRoles
            };
        }

        public int GetUserWalletBalance(string userId)
        {

            if (userId == null) return 0;

            int deposit = (int)_userRepository.GetDepositUserWallet(userId).Sum();

            int withdraw = (int)_userRepository.GetWithdrawUserWallet(userId).Sum();

            return (deposit - withdraw);
        }

        public IEnumerable<Domain.Entities.User.User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public IAsyncEnumerable<UserInformationViewModel> GetUsersInformation()
            =>
                _userRepository.GetUsersInformation();


        public async Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int productsCount = await _userRepository.GetShoppersCountAsync();

            var shoppers = _userRepository.GetShoppersWithPagination(skip, take);

            int totalPages = (int)Math.Ceiling(1.0 * productsCount / take);

            return new Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>(shoppers, pageId, totalPages);
        }

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

        public async Task<Domain.Entities.User.User> LoginUserAsync(LoginViewModel login)
        {
            string userName = login.UserName.Trim();
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);

            return null;
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _userRepository.AddUserRoleAsync(userRole);
            await _userRepository.SaveChangesAsync();
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

        public async Task RemoveUserRolesByUserIdAsync(string userId)
        {
            var userRoles = _userRepository.GetUserRolesByUserId(userId);
            if (userRoles != null)
            {
                foreach (var userRole in userRoles)
                {
                    _userRepository.RemoveUserRole(userRole);
                }

                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveUserAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            _userRepository.RemoveUser(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task RemoveUserRole(UserRole userRole)
        {
            _userRepository.RemoveUserRole(userRole);
            await _userRepository.SaveChangesAsync();
        }

        public IEnumerable<Role> GetRoles()
        {
            return _userRepository.GetRoles();
        }

        public async Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId)
        {
            var role = await _userRepository.GetRoleByIdAsync(roleId);

            return new AddOrEditRoleViewModel()
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
            };
        }

        public async Task AddRoleAsync(Role role)
        {
            await _userRepository.AddRoleAsync(role);
            await _userRepository.SaveChangesAsync();
        }

        public async Task EditRoleAsync(Role role)
        {
            _userRepository.EditRole(role);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _userRepository.GetRoleByIdAsync(roleId);
        }

        public async Task RemoveRoleAsync(string roleId)
        {
            var role = await _userRepository.GetRoleByIdAsync(roleId);

            _userRepository.RemoveRole(role);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> IsRoleExistAsync(string roleId)
        {
            return await _userRepository.IsRoleExistAsync(roleId);
        }

        public async Task<ResultTypes> AddUserToRoleAsync(Domain.Entities.User.User user, string roleName)
        {
            var role = await _userRepository.GetRoleByNameAsync(roleName);
            if (role is null)
                return ResultTypes.Failed;



            var userRole = new UserRole(user.UserId, role.RoleId);

            await _userRepository.AddUserRoleAsync(userRole);
            await _userRepository.SaveChangesAsync();

            return ResultTypes.Successful;
        }

        public async Task<ResultTypes> RemoveUserFromRoleAsync(Domain.Entities.User.User user, string roleName)
        {
            var role = await _userRepository.GetRoleByNameAsync(roleName);
            if (role is null)
                return ResultTypes.Failed;

            var userRole = await _userRepository.GetUserRoleAsync(user.UserId, role.RoleId);
            if (userRole is null)
                return ResultTypes.Failed;


            _userRepository.RemoveUserRole(userRole);
            await _userRepository.SaveChangesAsync();

            return ResultTypes.Successful;
        }
    }
}
