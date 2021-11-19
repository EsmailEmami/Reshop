using System;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.User;
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

        #region user

        public async Task<bool> IsPhoneExistAsync(string phone) => await _context.Users.AnyAsync(c => c.PhoneNumber == phone);

        public async Task<bool> IsUserExistAsync(string userId) => await _context.Users.AnyAsync(c => c.UserId == userId);
        public async Task AddAddressAsync(Address address) => await _context.Addresses.AddAsync(address);

        public void UpdateAddress(Address address) => _context.Addresses.Update(address);

        public void RemoveAddress(Address address) => _context.Addresses.Remove(address);

        public async Task<Domain.Entities.User.User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<EditUserViewModel> GetUserDataForEditAsync(string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .Select(c => new EditUserViewModel()
                {
                    UserId = c.UserId,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    NationalCode = c.NationalCode,
                }).SingleOrDefaultAsync();

        public async Task<UserDetailViewModel> GetUserDetailAsync(string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .Select(c => new UserDetailViewModel()
                {
                    UserId = c.UserId,
                    UserImage = c.UserAvatar,
                    FullName = c.FullName,
                    NationalCode = c.NationalCode,
                  AddressesCount = c.Addresses.Count,
                  PhoneNumber = c.PhoneNumber,
                  Email = c.Email,
                  OrdersCount = c.Orders.Count(o => o.IsReceived),
                  RegisterDate = c.RegisterDate,
                })
                .SingleOrDefaultAsync();


        public IEnumerable<Address> GetUserAddresses(string userId)
            =>
                _context.Addresses.Where(c => c.UserId == userId)
                    .Include(c => c.City)
                    .ThenInclude(c => c.State);

        public async Task AddUserAsync(Domain.Entities.User.User user) => await _context.Users.AddAsync(user);

        public async Task AddUserInviteAsync(UserInvite userInvite) => await _context.UserInvites.AddAsync(userInvite);

        public IAsyncEnumerable<Domain.Entities.User.User> GetUsers() => _context.Users;

        public IEnumerable<UserInformationForListViewModel> GetUsersInformation()
            =>
                _context.Users
                    .Select(c => new UserInformationForListViewModel(c.UserId, c.FullName, c.PhoneNumber));

        public async Task<Domain.Entities.User.User> GetUserByInviteCodeAsync(string inviteCode)
            =>
                await _context.Users.SingleOrDefaultAsync(c => c.InviteCode == inviteCode);

        public async Task<Domain.Entities.User.User> GetUserByIdAsync(string userId) => await _context.Users.FindAsync(userId);

        public void UpdateUser(Domain.Entities.User.User user) => _context.Users.Update(user);

        public void RemoveUser(Domain.Entities.User.User user) => _context.Users.Remove(user);

        #endregion

        #region wallet

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

        public async Task<Address> GetAddressByIdAsync(string addressId) =>
            await _context.Addresses.FindAsync(addressId);

        public async Task<bool> IsUserAddressExistAsync(string addressId, string userId) =>
            await _context.Addresses.AnyAsync(c => c.UserId == userId && c.AddressId == addressId);

        #endregion

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}