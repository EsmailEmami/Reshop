using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;
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

        Task<QuestionAndAnswerResultTypes> LikeQuestionAsync(QuestionLike questionLike);

        Task<ResultTypes> AddReportQuestionAsync(ReportQuestion reportQuestion);
        Task<ResultTypes> RemoveReportQuestionAsync(string userId, int questionId);
        IEnumerable<ReportQuestionType> GetReportQuestionTypes();

        // questionAnswer
        Task<ResultTypes> AddQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<ResultTypes> EditQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId);
        Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId);

        Task<QuestionAndAnswerResultTypes> LikeQuestionAnswerAsync(QuestionAnswerLike questionAnswerLike);

        Task<ResultTypes> AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer);
        Task<ResultTypes> RemoveReportQuestionAnswerAsync(string userId, int questionAnswerId);

        IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes();
    }
}