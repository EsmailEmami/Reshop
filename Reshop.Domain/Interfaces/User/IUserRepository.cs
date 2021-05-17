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

        #region GetAll

        IEnumerable<Domain.Entities.User.User> GetUsers();
        IAsyncEnumerable<UserInformationViewModel> GetUsersInformation();

        #endregion

        #region Get By Id

        Task<Entities.User.User> GetUserByActiveCodeAsync(string activeCode);
        Task<Entities.User.User> GetUserByInviteCodeAsync(string inviteCode);
        Task<Entities.User.User> GetUserByIdAsync(string userId);
        IEnumerable<UserRole> GetUserRolesByUserId(string userId);
        IEnumerable<string> GetRolesIdOfUser(string userId);

        IAsyncEnumerable<Address> GetUserAddresses(string userId);

        #endregion

        #region Insert

        Task AddUserAsync(Entities.User.User user);
        Task AddUserRoleAsync(UserRole userRole);
        Task AddUserInviteAsync(UserInvite userInvite);

        #endregion

        #region Update

        // update
        void UpdateUser(Entities.User.User user);

        #endregion

        #region Remove

        void RemoveUserRole(UserRole userRole);
        void RemoveUser(Entities.User.User user);

        #endregion

        #region Validation

        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsUserExistAsync(string userId);

        #endregion

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

        #region Role

        IEnumerable<Role> GetRoles();
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);

        // insert
        Task AddRoleAsync(Role role);
        Task<UserRole> GetUserRoleAsync(string userId, string roleId);

        // update
        void EditRole(Role role);

        // validations
        Task<bool> IsRoleExistAsync(string roleId);

        // remove
        void RemoveRole(Role role);

        #endregion

        #region Shopper

        IAsyncEnumerable<UserInformationViewModel> GetShoppers();
        Task<Entities.User.User> GetShopperByUserIdAsync(string userId);
        Task AddShopperAsync(Entities.Shopper.Shopper shopper);
        void RemoveShopper(Entities.Shopper.Shopper shopper);
        void UpdateShopper(Entities.Shopper.Shopper shopper);
        Task AddShopperProduct(ShopperProduct shopperProduct);
        Task<ShopperProduct> GetShopperProductAsync(string shopperUserId, int productId);
        Task<int> GetShoppersCountAsync();
        IAsyncEnumerable<ShopperInformationViewModel> GetShoppersWithPagination(int skip = 0, int take = 24);

        void UpdateShopperProduct(ShopperProduct shopperProduct);

        #endregion

        Task SaveChangesAsync();
        void SaveChanges();
    }
}