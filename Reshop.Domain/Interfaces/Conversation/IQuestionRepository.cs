using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Question;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.Conversation
{
    public interface IQuestionRepository
    {
        // question
        IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId);
        Task AddQuestionAsync(Question question);
        void UpdateQuestion(Question question);
        void RemoveQuestion(Question question);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<EditQuestionViewModel> GetQuestionDataForEditAsync(int questionId);
        Task<int> GetQuestionsCountOfProductWithTypeAsync(int productId);
        IEnumerable<ProductQuestionsForShow> GetProductQuestionsWithPagination(int productId, int skip = 1, int take = 30, string type = "news");

        Task AddReportQuestionAsync(ReportQuestion reportQuestion);
        Task<bool> IsReportQuestionExistAsync(string userId, int questionId);
        IEnumerable<ReportQuestionType> GetReportQuestionTypes();

        // questionAnswer
        Task AddQuestionAnswerAsync(QuestionAnswer questionAnswer);
        void UpdateQuestionAnswer(QuestionAnswer questionAnswer);
        void RemoveQuestionAnswer(QuestionAnswer questionAnswer);
        Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId);
        Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId);

        Task AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer);
        Task<bool> IsReportQuestionAnswerExistAsync(string userId, int questionAnswerId);
        IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes();

        Task SaveChangesAsync();
    }
}