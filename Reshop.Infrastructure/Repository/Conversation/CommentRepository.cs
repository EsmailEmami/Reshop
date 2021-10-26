using Microsoft.EntityFrameworkCore;
using Reshop.Application.Calculate;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Conversation
{
    public class CommentRepository : ICommentRepository
    {
        #region constructor 

        private readonly ReshopDbContext _context;

        public CommentRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<int> GetCommentsCountOfProductWithTypeAsync(int productId, string type = "all") =>
            type switch
            {
                "all" => await _context.Comments
                    .Where(c => c.ProductId == productId)
                    .CountAsync(),
                "buyers" => await _context.Comments
                    .Where(c => c.ProductId == productId && !string.IsNullOrEmpty(c.ShopperProductColorId))
                    .CountAsync(),
                _ => await _context.Comments
                    .Where(c => c.ProductId == productId)
                    .CountAsync()
            };

        public IEnumerable<ProductCommentsForShow> GetProductCommentsWithPagination(int productId, int skip = 1, int take = 30, string type = "news")
        {
            IQueryable<Comment> comments = _context.Comments
                .Where(c => c.ProductId == productId && !c.IsDelete);

            comments = type switch
            {
                "news" => comments.OrderByDescending(c => c.CommentDate),
                "buyers" => comments.Where(c => !string.IsNullOrEmpty(c.ShopperProductColorId))
                    .OrderByDescending(c => c.CommentDate),
                "best" => comments.OrderByDescending(c => c.CommentFeedBacks.Count(f => f.Type)),
                _ => comments.OrderByDescending(c => c.CommentDate)
            };

            comments = comments.Skip(skip).Take(take);

            return comments.Select(c => new ProductCommentsForShow()
            {
                CommentId = c.CommentId,
                FullName = c.User.FullName,
                Image = c.User.UserAvatar,
                CommentDate = c.CommentDate,
                CommentTitle = c.CommentTitle,
                CommentText = c.CommentText,
                StoreName = c.ShopperProductColor.ShopperProduct.Shopper.StoreName,
                ColorName = c.ShopperProductColor.Color.ColorName,
                ProductShortKey = c.ShopperProductColor.ShortKey,
                FeedBacks = c.CommentFeedBacks.Select(f => f.Type)
            });
        }

        public async Task<CommentsOfProductDetailViewModel> GetCommentsOfProductDetailAsync(int productId)
        {
            IQueryable<Comment> comments = _context.Products
                .Where(c => c.ProductId == productId)
                .SelectMany(c => c.Comments);


            int productSatisfaction = 0;
            int constructionQuality = 0;
            int featuresAndCapabilities = 0;
            int designAndAppearance = 0;
            int suggestedCommentsCounts = await comments.Where(c => c.OverallScore >= 50).CountAsync();
            int suggestedBuyersCounts = await comments.Where(c => c.OverallScore >= 50 && c.ShopperProductColorId != null).CountAsync();
            int allCommentsCount = await comments.CountAsync();


            int commentsPercent = CommentCalculator.CalculatePercentOfTwoNumber(suggestedCommentsCounts, allCommentsCount);

            double commentsScore = CommentCalculator.CommentScore(commentsPercent);

            if (comments.Any())
            {
                productSatisfaction = (int)await comments.AverageAsync(c => c.ProductSatisfaction);
                constructionQuality = (int)await comments.AverageAsync(c => c.ConstructionQuality);
                featuresAndCapabilities = (int)await comments.AverageAsync(c => c.FeaturesAndCapabilities);
                designAndAppearance = (int)await comments.AverageAsync(c => c.DesignAndAppearance);
            }




            return new CommentsOfProductDetailViewModel()
            {
                AllCommentsCount = allCommentsCount,
                CommentsScore = commentsScore,
                SuggestedBuyersCounts = suggestedBuyersCounts,
                SuggestedCommentsCounts = suggestedCommentsCounts,
                ProductSatisfaction = productSatisfaction,
                ConstructionQuality = constructionQuality,
                FeaturesAndCapabilities = featuresAndCapabilities,
                DesignAndAppearance = designAndAppearance
            };
        }

        public async Task AddCommentAsync(Comment comment) =>
            await _context.Comments.AddAsync(comment);

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
    }
}