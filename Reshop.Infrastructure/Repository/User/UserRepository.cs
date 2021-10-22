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


        public IEnumerable<Address> GetUserAddresses(string userId)
            =>
                _context.Addresses.Where(c => c.UserId == userId)
                    .Include(c => c.City)
                    .ThenInclude(c => c.State);

        public async Task AddUserAsync(Domain.Entities.User.User user) => await _context.Users.AddAsync(user);

        public async Task AddUserInviteAsync(UserInvite userInvite) => await _context.UserInvites.AddAsync(userInvite);

        public IAsyncEnumerable<Domain.Entities.User.User> GetUsers() => _context.Users;

        public IEnumerable<UserInformationViewModel> GetUsersInformation()
            =>
                _context.Users
                    .Select(c => new UserInformationViewModel(c.UserId, c.FullName, c.PhoneNumber));

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

        public IEnumerable<Tuple<int, bool>> GetUserProductCommentsFeedBack(string userId, int productId) =>
            _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.CommentFeedBacks)
                .Where(c => c.Comment.ProductId == productId)
                .Select(c => new Tuple<int, bool>(c.CommentId, c.Type));

        public async Task<bool> IsUserReportCommentExistAsync(string userId, int commentId) =>
            await _context.ReportComments.AnyAsync(c => c.UserId == userId && c.CommentId == commentId);

        public async Task AddReportCommentAsync(ReportComment reportComment) =>
            await _context.ReportComments.AddAsync(reportComment);

        public void RemoveReportComment(ReportComment reportComment) =>
            _context.ReportComments.Remove(reportComment);

        public async Task<ReportComment> GetReportCommentAsync(string userId, int commentId) =>
            await _context.ReportComments.SingleOrDefaultAsync(c => c.UserId == userId && c.CommentId == commentId);

        public IEnumerable<ReportCommentType> GetReportCommentTypes() =>
            _context.ReportCommentTypes;

        public async Task<bool> IsReportCommentTimeLockAsync(string userId, int commentId) =>
            await _context.ReportComments
                .AnyAsync(c => c.UserId == userId && c.CommentId == commentId && c.CreateDate >= DateTime.Now.AddMinutes(-2));

        public IEnumerable<int> GetUserReportCommentsOfProduct(string userId, int productId) =>
            _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.ReportComments)
                .Where(c => c.Comment.ProductId == productId)
                .Select(c => c.CommentId);

        public async Task<CommentFeedback> GetCommentFeedBackAsync(string userId, int commentId) =>
            await _context.CommentFeedBacks.SingleOrDefaultAsync(c => c.UserId == userId && c.CommentId == commentId);

        public async Task AddCommentFeedBackAsync(CommentFeedback commentFeedback) =>
            await _context.CommentFeedBacks.AddAsync(commentFeedback);

        public void RemoveCommentFeedBack(CommentFeedback commentFeedback) =>
            _context.CommentFeedBacks.Remove(commentFeedback);

        public void UpdateCommentFeedBack(CommentFeedback commentFeedback) =>
            _context.CommentFeedBacks.Update(commentFeedback);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void SaveChanges() => _context.SaveChanges();
    }
}