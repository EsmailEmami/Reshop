using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;

namespace Reshop.Application.Interfaces.Conversation
{
    public interface IQuestionService
    {
        // question
        Task<ResultTypes> AddQuestionAsync(Question question);
        Task<ResultTypes> EditQuestionAsync(Question question);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<EditQuestionViewModel> GetQuestionDataForEditAsync(int questionId);

        Task<ResultTypes> AddReportQuestionAsync(ReportQuestion reportQuestion);
        IEnumerable<ReportQuestionType> GetReportQuestionTypes();

        // questionAnswer
        Task<ResultTypes> AddQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<ResultTypes> EditQuestionAnswerAsync(QuestionAnswer questionAnswer);
        Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId);
        Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId);

        Task<ResultTypes> AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer);
        IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes();
    }
}