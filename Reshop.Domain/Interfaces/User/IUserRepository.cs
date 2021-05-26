using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.User
{
    public interface IUserRepository
    {
        #region User

        IAsyncEnumerable<Entities.User.User> GetUsers();
        IAsyncEnumerable<UserInformationViewModel> GetUsersInformation();
        Task<Entities.User.User> GetUserByActiveCodeAsync(string activeCode);
        Task<Entities.User.User> GetUserByInviteCodeAsync(string inviteCode);
        Task<Entities.User.User> GetUserByIdAsync(string userId);
        Task<Entities.User.User> GetUserByPhoneNumberAsync(string phoneNumber);
        IAsyncEnumerable<Address> GetUserAddresses(string userId);
        Task AddUserAsync(Entities.User.User user);
        Task AddUserInviteAsync(UserInvite userInvite);
        void UpdateUser(Entities.User.User user);
        void RemoveUser(Entities.User.User user);
        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsUserExistAsync(string userId);

        #endregion

        #region Wallet

        IEnumerable<decimal> GetDepositUserWallet(string userId);
        IEnumerable<decimal> GetWithdrawUserWallet(string userId);

        #endregion

        #region Discount

        Discount GetDiscountByCode(string discountCode);
        Task<Discount> GetDiscountByCodeAsync(string discountCode);
        void UpdateDiscount(Discount discount);
        bool IsUserDiscountCodeExist(string userId, string discountId);
        Task AddUserDiscountCodeAsync(UserDiscountCode userDiscountCode);

        #endregion

        Task SaveChangesAsync();
        void SaveChanges();
    }
}