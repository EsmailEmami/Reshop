using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Comment;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.Conversation;

namespace Reshop.Application.Services.Conversation
{
    public class CommentService : ICommentService
    {
        #region constructor

        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        #endregion

        public async Task<Tuple<IEnumerable<ProductCommentsForShow>, int, int>> GetProductCommentsWithPaginationAsync(int productId, int pageId = 1, int take = 30, string type = "news")
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int count = await _commentRepository.GetCommentsCountOfProductWithTypeAsync(productId, type.FixedText());

            var data = _commentRepository.GetProductCommentsWithPagination(productId, skip, take, type.FixedText());

            int totalPages = (int)Math.Ceiling(1.0 * count / take);

            return new Tuple<IEnumerable<ProductCommentsForShow>, int, int>(data, pageId, totalPages);
        }

        public async Task<ResultTypes> AddCommentAsync(Comment comment)
        {
            try
            {
                await _commentRepository.AddCommentAsync(comment);

                await _commentRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch 
            { 
                return ResultTypes.Failed;
            }

        }

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserCommentsForShow(string userId) =>
         _commentRepository.GetUserCommentsForShow(userId);

        public IEnumerable<Tuple<int, bool>> GetUserProductCommentsFeedBack(string userId, int productId) =>
            _commentRepository.GetUserProductCommentsFeedBack(userId, productId);

        public async Task<ResultTypes> AddReportCommentAsync(ReportComment reportComment)
        {
            try
            {
                if (await _commentRepository.IsUserReportCommentExistAsync(reportComment.UserId, reportComment.CommentId))
                    return ResultTypes.Failed;

                await _commentRepository.AddReportCommentAsync(reportComment);

                await _commentRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveReportCommentByUserAsync(string userId, int commentId)
        {
            try
            {
                var reportComment = await _commentRepository.GetReportCommentAsync(userId, commentId);

                if (reportComment == null)
                    return ResultTypes.Failed;


                _commentRepository.RemoveReportComment(reportComment);

                await _commentRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<ReportCommentType> GetReportCommentTypes() =>
            _commentRepository.GetReportCommentTypes();

        public async Task<bool> IsReportCommentTimeLockAsync(string userId, int commentId) =>
            await _commentRepository.IsReportCommentTimeLockAsync(userId, commentId);

        public IEnumerable<int> GetUserReportCommentsOfProduct(string userId, int productId) =>
             _commentRepository.GetUserReportCommentsOfProduct(userId, productId);

        public async Task<CommentFeedBackType> AddCommentFeedBackAsync(string userId, int commentId, string type)
        {
            try
            {
                var commentFeedBack = await _commentRepository.GetCommentFeedBackAsync(userId, commentId);

                bool boolType;

                if (type.FixedText() == "like")
                {
                    boolType = true;
                }
                else if (type.FixedText() == "dislike")
                {
                    boolType = false;
                }
                else
                {
                    return CommentFeedBackType.Error;
                }


                // user have no feedback of comment
                if (commentFeedBack == null)
                {
                    var newCommentFeedBack = new CommentFeedback()
                    {
                        CommentId = commentId,
                        UserId = userId,
                        Type = boolType
                    };

                    await _commentRepository.AddCommentFeedBackAsync(newCommentFeedBack);
                    await _commentRepository.SaveChangesAsync();


                    if (boolType)
                    {
                        return CommentFeedBackType.LikeAdded;
                    }
                    else
                    {
                        return CommentFeedBackType.DislikeAdded;
                    }
                }
                else
                {
                    if (commentFeedBack.Type == boolType)
                    {
                        _commentRepository.RemoveCommentFeedBack(commentFeedBack);
                        await _commentRepository.SaveChangesAsync();

                        await _commentRepository.SaveChangesAsync();

                        if (boolType)
                        {
                            return CommentFeedBackType.LikeRemoved;
                        }
                        else
                        {
                            return CommentFeedBackType.DislikeRemoved;
                        }
                    }
                    else
                    {
                        commentFeedBack.Type = boolType;
                        _commentRepository.UpdateCommentFeedBack(commentFeedBack);
                        await _commentRepository.SaveChangesAsync();

                        if (boolType)
                        {
                            return CommentFeedBackType.LikeEdited;
                        }
                        else
                        {
                            return CommentFeedBackType.DislikeEdited;
                        }
                    }
                }
            }
            catch
            {
                return CommentFeedBackType.Error;
            }
        }
    }
}