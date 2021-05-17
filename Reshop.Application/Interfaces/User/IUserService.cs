using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.User
{
    public interface IUserService
    {
        #region User

        // get
        Task<AddOrEditUserViewModel> GetUserDataAsync(string userId);
        int GetUserWalletBalance(string userId);
        IEnumerable<Domain.Entities.User.User> GetUsers();
        IAsyncEnumerable<UserInformationViewModel> GetUsersInformation();
        Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18);
        IAsyncEnumerable<Address> GetUserAddresses(string userId);

        // validations
        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsUserExistAsync(string userId);

        // insert
        Task<ResultTypes> AddUserAsync(Domain.Entities.User.User user);
        Task<Domain.Entities.User.User> LoginUserAsync(LoginViewModel login);
        Task AddUserToInvitesAsync(string inviteCode, string invitedUserId);
        Task AddUserRoleAsync(UserRole userRole);

        // update
        Task<ResultTypes> EditUserAsync(Domain.Entities.User.User user);

        Task<Domain.Entities.User.User> GetUserByIdAsync(string userId);

        // remove
        Task RemoveUserRolesByUserIdAsync(string userId);
        Task RemoveUserAsync(string userId);

        #endregion

        #region Role

        Task<ResultTypes> AddUserToRoleAsync(Domain.Entities.User.User user, string roleName);
        Task<ResultTypes> RemoveUserFromRoleAsync(Domain.Entities.User.User user, string roleName);
        IEnumerable<Role> GetRoles();
        Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId);
        Task AddRoleAsync(Role role);
        Task EditRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task RemoveRoleAsync(string roleId);
        Task<bool> IsRoleExistAsync(string roleId);

        #endregion
    }
}