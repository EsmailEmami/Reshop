using Reshop.Application.Enums;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.User
{
    public interface IUserService
    {


        Task<AddOrEditUserViewModel> GetUserDataAsync(string userId);
        int GetUserWalletBalance(string userId);
        IAsyncEnumerable<Domain.Entities.User.User> GetUsers();
        IAsyncEnumerable<UserInformationViewModel> GetUsersInformation();
        //Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18);
        IAsyncEnumerable<Address> GetUserAddresses(string userId);
        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsUserExistAsync(string userId);
        Task<ResultTypes> AddUserAsync(Domain.Entities.User.User user);
        Task<Domain.Entities.User.User> LoginUserAsync(LoginViewModel login);
        Task AddUserToInvitesAsync(string inviteCode, string invitedUserId);
        Task<ResultTypes> EditUserAsync(Domain.Entities.User.User user);
        Task<Domain.Entities.User.User> GetUserByIdAsync(string userId);
        Task<ResultTypes> RemoveUserAsync(string userId);

    }
}