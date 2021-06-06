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
            return await _context.Users.SingleOrDefaultAsync(c=> c.PhoneNumber == phoneNumber);
        }

        public IAsyncEnumerable<Address> GetUserAddresses(string userId)
            =>
                _context.Addresses.Where(c=> c.UserId == userId) as IAsyncEnumerable<Address>;

        public async Task AddUserAsync(Domain.Entities.User.User user) => await _context.Users.AddAsync(user);

        public async Task AddUserInviteAsync(UserInvite userInvite) => await _context.UserInvites.AddAsync(userInvite);

        public IAsyncEnumerable<Domain.Entities.User.User> GetUsers() => _context.Users;

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

        #endregion

        #region discount

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
            =>
                await _context.UserDiscountCodes.AddAsync(userDiscountCode);

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId) =>
            _context.Questions.Where(c => c.UserId == userId)
                .Select(c => new ShowQuestionOrCommentViewModel()
                {
                    QuestionOrCommentId = c.QuestionId,
                    QuestionOrCommentTitle = c.QuestionTitle,
                    SentDate = c.QuestionDate
                });

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId) =>
            _context.Comments.Where(c => c.UserId == userId)
                .Select(c => new ShowQuestionOrCommentViewModel()
                {
                    QuestionOrCommentId = c.CommentId,
                    QuestionOrCommentTitle = c.CommentTitle,
                    SentDate = c.CommentDate
                });

        #endregion

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void SaveChanges() => _context.SaveChanges();
    }
}