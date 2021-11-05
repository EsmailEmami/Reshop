using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.Conversation
{
    public interface IQuestionService
    {
        // question
        Task<ResultTypes> AddQuestionAsync(Question question);
        Task<ResultTypes> EditQuestionAsync(Question question);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<EditQuestionViewModel> GetQuestionDataForEditAsync(int questionId);
        Task<bool> IsQuestionRemovableAsync(int questionId);
        Task<bool> IsUserQuestionAsync(string userId, int questionId);

        Task<Tuple<IEnumerable<ProductQuestionsForShow>, int, int>> GetProductQuestionsWithPaginationAsync(int productId, int pageId = 1, int take = 30, string type = "news", string filter = "");

        Task<ResultTypes> DeleteQuestionAsync(int questionId, string description);

        Task<QuestionAndAnswerResultTypes> LikeQuestionAsync(string userId, int questionId);

        Task<ResultTypes> AddReportQuestionAsync(ReportQuestion reportQuestion);
        Task<ResultTypes> RemoveReportQuestionAsync(string userId, int questionId);
        IEnumerable<ReportQuestionType> GetReportQuestionTypes();


        Task<IEnumerable<int>> GetUserQuestionLikesOfProductAsync(int productId, string userId);
        Task<IEnumerable<int>> GetUserQuestionReportsOfProductAsync(int productId, string userId);

        Task<IEnumerable<int>> GetUserQuestionAnswerLikesOfProductAsync(int productId, string userId);
        Task<IEnumerable<int>> GetUserQuestionAnswerReportsOfProductAsync(int productId, string userId);



        // questionAnswer
        Task<ResultTypes> AddQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<ResultTypes> EditQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId);
        Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId);

        Task<QuestionAndAnswerResultTypes> LikeQuestionAnswerAsync(QuestionAnswerLike questionAnswerLike);

        Task<ResultTypes> AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer);
        Task<ResultTypes> RemoveReportQuestionAnswerAsync(string userId, int questionAnswerId);

        Task<bool> IsQuestionAnswerRemovableAsync(int questionAnswerId);
        Task<bool> IsUserQuestionAnswerAsync(string userId, int questionAnswerId);

        Task<ResultTypes> DeleteQuestionAnswerAsync(int questionAnswerId, string description);

        IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes();
    }
}