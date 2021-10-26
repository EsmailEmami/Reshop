using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Comment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Conversation
{
    public interface ICommentRepository
    {
        Task<int> GetCommentsCountOfProductWithTypeAsync(int productId, string type = "all");
        IEnumerable<ProductCommentsForShow> GetProductCommentsWithPagination(int productId, int skip = 1, int take = 30, string type = "news");
        Task<CommentsOfProductDetailViewModel> GetCommentsOfProductDetailAsync(int productId);
        Task AddCommentAsync(Comment comment);

        IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId);
        IEnumerable<Tuple<int, bool>> GetUserProductCommentsFeedBack(string userId, int productId);
        Task<bool> IsUserReportCommentExistAsync(string userId, int commentId);
        Task AddReportCommentAsync(ReportComment reportComment);
        void RemoveReportComment(ReportComment reportComment);
        Task<ReportComment> GetReportCommentAsync(string userId, int commentId);
        IEnumerable<ReportCommentType> GetReportCommentTypes();
        Task<bool> IsReportCommentTimeLockAsync(string userId, int commentId);
        IEnumerable<int> GetUserReportCommentsOfProduct(string userId, int productId);
        Task<CommentFeedback> GetCommentFeedBackAsync(string userId, int commentId);
        Task AddCommentFeedBackAsync(CommentFeedback commentFeedback);
        void RemoveCommentFeedBack(CommentFeedback commentFeedback);
        void UpdateCommentFeedBack(CommentFeedback commentFeedback);

        Task SaveChangesAsync();
    }
}