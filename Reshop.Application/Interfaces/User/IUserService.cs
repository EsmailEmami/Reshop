using Reshop.Application.Enums;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.CommentAndQuestion;

namespace Reshop.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<AddOrEditUserForAdminViewModel> GetUserDataForAdminAsync(string userId);

        Task<EditUserViewModel> GetUserDataForEditAsync(string userId);

        Task<UserDetailViewModel> GetUserDetailAsync(string userId);

        int GetUserWalletBalance(string userId);
        IEnumerable<Domain.Entities.User.User> GetUsers();
        IEnumerable<UserInformationForListViewModel> GetUsersInformation();
        //Task<Tuple<IAsyncEnumerable<ShopperInformationViewModel>, int, int>> GetShoppersInformationWithPagination(int pageId = 1, int take = 18);
        IEnumerable<Address> GetUserAddresses(string userId);
        IEnumerable<AddressForShowViewModel> GetUserAddressesForShow(string userId);
        Task<bool> IsPhoneNumberExistAsync(string phoneNumber);
        Task<bool> IsUserPasswordValidAsync(string userId, string password);
        Task<bool> IsUserExistAsync(string userId);
        Task<int> GetUserAddressesCountAsync(string userId);
        Task<ResultTypes> AddUserAsync(Domain.Entities.User.User user);
        Task<Domain.Entities.User.User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task AddUserToInvitesAsync(string inviteCode, string invitedUserId);
        Task<ResultTypes> EditUserAsync(Domain.Entities.User.User user);
        Task<Domain.Entities.User.User> GetUserByIdAsync(string userId);
        Task<ResultTypes> RemoveUserAsync(string userId);


        IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId);
  
        #region address

        Task<ResultTypes> AddUserAddressAsync(Address address);
        Task<ResultTypes> EditUserAddressAsync(Address address);
        Task<ResultTypes> DeleteUserAddressAsync(string addressId);
        Task<ResultTypes> DeleteUserAddressAsync(Address address);
        Task<Address> GetAddressByIdAsync(string addressId);

        Task<bool> IsUserAddressExistAsync(string addressId, string userId);

        #endregion

    }
}