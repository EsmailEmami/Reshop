using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Comment;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.Conversation
{
    public interface ICommentService
    {
        Task<Tuple<IEnumerable<ProductCommentsForShow>, int, int>> GetProductCommentsWithPaginationAsync(int productId, int pageId = 1, int take = 30, string type = "news");
        Task<ResultTypes> AddCommentAsync(Comment comment);
        IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId);
        IEnumerable<Tuple<int, bool>> GetUserProductCommentsFeedBack(string userId, int productId);
        Task<ResultTypes> ReportCommentByUserAsync(string userId, AddReportCommentViewModel model);
        Task<ResultTypes> RemoveReportCommentByUserAsync(string userId, int commentId);
        IEnumerable<ReportCommentType> GetReportCommentTypes();
        Task<bool> IsReportCommentTimeLockAsync(string userId, int commentId);
        IEnumerable<int> GetUserReportCommentsOfProduct(string userId, int productId);
        Task<CommentFeedBackType> AddCommentFeedBackAsync(string userId, int commentId, string type);

    }
}