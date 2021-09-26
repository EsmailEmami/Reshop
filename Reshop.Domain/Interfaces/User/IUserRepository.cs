﻿using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.User
{
    public interface IUserRepository
    {
        #region User

        IAsyncEnumerable<Entities.User.User> GetUsers();
        IEnumerable<UserInformationViewModel> GetUsersInformation();
        Task<Entities.User.User> GetUserByInviteCodeAsync(string inviteCode);
        Task<Entities.User.User> GetUserByIdAsync(string userId);
        Task<Entities.User.User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<EditUserViewModel> GetUserDataForEditAsync(string userId);

        Task AddUserAsync(Entities.User.User user);
        Task AddUserInviteAsync(UserInvite userInvite);
        void UpdateUser(Entities.User.User user);
        void RemoveUser(Entities.User.User user);
        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsUserExistAsync(string userId);


        Task AddAddressAsync(Address address);
        void UpdateAddress(Address address);
        void RemoveAddress(Address address);
        IEnumerable<Address> GetUserAddresses(string userId);

        #endregion

        #region Wallet

        IEnumerable<decimal> GetDepositUserWallet(string userId);
        IEnumerable<decimal> GetWithdrawUserWallet(string userId);

        #endregion

        #region address

        Task<Address> GetAddressByIdAsync(string addressId);
        Task<bool> IsUserAddressExistAsync(string addressId, string userId);
        #endregion

        #region Discount

        Entities.User.Discount GetDiscountByCode(string discountCode);
        Task<Entities.User.Discount> GetDiscountByCodeAsync(string discountCode);
        void UpdateDiscount(Entities.User.Discount discount);
        bool IsUserDiscountCodeExist(string userId, string discountId);
        Task AddUserDiscountCodeAsync(UserDiscountCode userDiscountCode);

        #endregion

        #region comment and question

        IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId);
        IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId);

        #endregion

        Task SaveChangesAsync();
        void SaveChanges();
    }
}