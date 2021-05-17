using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.User
{
    public class UserRepository : IUserRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public UserRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<bool> IsPhoneExistAsync(string phone) => await _context.Users.AnyAsync(c => c.PhoneNumber == phone);

        public async Task<bool> IsUserExistAsync(string userId) => await _context.Users.AnyAsync(c => c.UserId == userId);

        public IAsyncEnumerable<Address> GetUserAddresses(string userId)
        {
            return _context.Users.Where(c => c.UserId == userId)
                .Include(c => c.Addresses)
                .Select(c => c.Addresses) as IAsyncEnumerable<Address>;
        }

        public async Task AddUserAsync(Domain.Entities.User.User user) => await _context.Users.AddAsync(user);

        public async Task AddUserRoleAsync(UserRole userRole) => await _context.UserRoles.AddAsync(userRole);

        public async Task AddUserInviteAsync(UserInvite userInvite) => await _context.UserInvites.AddAsync(userInvite);

        public IEnumerable<Domain.Entities.User.User> GetUsers() => _context.Users;

        public IAsyncEnumerable<UserInformationViewModel> GetUsersInformation()
            =>
                _context.Users.Select(c => new UserInformationViewModel(c.UserId, c.FullName, c.PhoneNumber))
                    as IAsyncEnumerable<UserInformationViewModel>;

        public async Task<Domain.Entities.User.User> GetUserByActiveCodeAsync(string activeCode)
            =>
                await _context.Users.SingleOrDefaultAsync(c => c.ActiveCode == activeCode);

        public async Task<Domain.Entities.User.User> GetUserByInviteCodeAsync(string inviteCode)
            =>
                await _context.Users.SingleOrDefaultAsync(c => c.InviteCode == inviteCode);

        public async Task<Domain.Entities.User.User> GetUserByIdAsync(string userId) => await _context.Users.FindAsync(userId);

        public IEnumerable<UserRole> GetUserRolesByUserId(string userId) => _context.UserRoles.Where(c => c.UserId == userId);

        public IEnumerable<string> GetRolesIdOfUser(string userId)
            =>
                _context.UserRoles.Where(c => c.UserId == userId)
                    .Select(c => c.RoleId);

        public IEnumerable<decimal> GetDepositUserWallet(string userId)
            =>
                _context.Wallets
                    .Where(c => c.UserId == userId && c.WalletTypeId == 2 && c.IsPayed)
                    .Select(c => c.Amount);

        public IEnumerable<decimal> GetWithdrawUserWallet(string userId)
            =>
                _context.Wallets
                    .Where(c => c.UserId == userId && c.WalletTypeId == 1 && c.IsPayed)
                    .Select(c => c.Amount);

        public void UpdateUser(Domain.Entities.User.User user) => _context.Users.Update(user);

        public void RemoveUserRole(UserRole userRole) => _context.UserRoles.Remove(userRole);

        public void RemoveUser(Domain.Entities.User.User user) => _context.Users.Remove(user);

        public Discount GetDiscountByCode(string discountCode)
            =>
                _context.Discounts.SingleOrDefault(c => c.DiscountCode == discountCode);

        public async Task<Discount> GetDiscountByCodeAsync(string discountCode)
            =>
                await _context.Discounts.SingleOrDefaultAsync(c => c.DiscountCode == discountCode);

        public void UpdateDiscount(Discount discount) => _context.Discounts.Update(discount);

        public bool IsUserDiscountCodeExist(string userId, string discountId)
            =>
                _context.UserDiscountCodes.Any(c => c.UserId == userId && c.DiscountId == discountId);

        public async Task AddUserDiscountCodeAsync(UserDiscountCode userDiscountCode)
        {
            await _context.UserDiscountCodes.AddAsync(userDiscountCode);
        }

        public IEnumerable<Role> GetRoles() => _context.Roles;

        public async Task<Role> GetRoleByIdAsync(string roleId) => await _context.Roles.FindAsync(roleId);

        public async Task<Role> GetRoleByNameAsync(string roleName)
            =>
                await _context.Roles.SingleOrDefaultAsync(c => c.RoleTitle == roleName);


        public async Task AddRoleAsync(Role role) => await _context.Roles.AddAsync(role);

        public async Task<UserRole> GetUserRoleAsync(string userId, string roleId)
            =>
                await _context.UserRoles.SingleOrDefaultAsync(c => c.UserId == userId && c.RoleId == roleId);

        public void EditRole(Role role) => _context.Roles.Update(role);

        public async Task<bool> IsRoleExistAsync(string roleId) => await _context.Roles.AnyAsync(c => c.RoleId == roleId);

        public void RemoveRole(Role role) => _context.Roles.Remove(role);

        public IAsyncEnumerable<UserInformationViewModel> GetShoppers()
            =>
                _context.Users.Where(c => c.ShopperId != null)
                    .Select(c => new UserInformationViewModel(c.UserId, c.FullName, c.PhoneNumber)) as IAsyncEnumerable<UserInformationViewModel>;

        public async Task<Domain.Entities.User.User> GetShopperByUserIdAsync(string userId)
            =>
                await _context.Users.Where(c => c.UserId == userId)
                    .Include(c => c.Shopper)
                    .ThenInclude(c => c.ShopperProducts)
                    .SingleOrDefaultAsync();


        public async Task AddShopperAsync(Domain.Entities.Shopper.Shopper shopper) => await _context.Shoppers.AddAsync(shopper);

        public void RemoveShopper(Domain.Entities.Shopper.Shopper shopper) => _context.Shoppers.Remove(shopper);

        public void UpdateShopper(Domain.Entities.Shopper.Shopper shopper) => _context.Shoppers.Update(shopper);

        public async Task AddShopperProduct(ShopperProduct shopperProduct) => await _context.ShopperProducts.AddAsync(shopperProduct);

        public async Task<ShopperProduct> GetShopperProductAsync(string shopperUserId, int productId)
            =>
                await _context.ShopperProducts
                    .SingleOrDefaultAsync(c => c.ShopperUserId == shopperUserId && c.ProductId == productId);

        public async Task<int> GetShoppersCountAsync()
            =>
                await _context.Shoppers.CountAsync();

        public IAsyncEnumerable<ShopperInformationViewModel> GetShoppersWithPagination(int skip = 0, int take = 24)
            =>
                _context.Users.Where(c => c.ShopperId != null)
                    .Skip(skip).Take(take)
                    .Include(c => c.Shopper)
                    .Select(c => new ShopperInformationViewModel()
                    {
                        UserId = c.UserId,
                        FullName = c.FullName,
                        PhoneNumber = c.PhoneNumber,
                        Condition = c.Shopper.IsApproved
                    }) as IAsyncEnumerable<ShopperInformationViewModel>;

        public void UpdateShopperProduct(ShopperProduct shopperProduct) => _context.ShopperProducts.Update(shopperProduct);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void SaveChanges() => _context.SaveChanges();
    }
}