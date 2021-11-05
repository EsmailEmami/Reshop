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
        Task<bool> IsQuestionRemovableAsync(int questionId);
        Task<int> GetQuestionsCountOfProductAsync(int productId);
        Task<IEnumerable<ProductQuestionsForShow>> GetProductQuestionsWithPaginationAsync(int productId, int skip = 1, int take = 30, string type = "news");

        Task<bool> IsUserQuestionAsync(string userId, int questionId);

        Task<QuestionLike> GetQuestionLikeAsync(string userId, int questionId);
        Task AddQuestionLikeAsync(QuestionLike questionLike);
        void RemoveQuestionLike(QuestionLike questionLike);
        Task<bool> IsQuestionLikeExistAsync(string userId, int questionId);

        Task AddReportQuestionAsync(ReportQuestion reportQuestion);
        Task<bool> IsReportQuestionExistAsync(string userId, int questionId);
        Task<ReportQuestion> GetReportQuestionAsync(string userId, int questionId);
        void RemoveReportQuestion(ReportQuestion reportQuestion);

        IEnumerable<ReportQuestionType> GetReportQuestionTypes();


        Task<IEnumerable<int>> GetUserQuestionLikesOfProductAsync(int productId, string userId);
        Task<IEnumerable<int>> GetUserQuestionReportsOfProductAsync(int productId, string userId);

        Task<IEnumerable<int>> GetUserQuestionAnswerLikesOfProductAsync(int productId, string userId);
        Task<IEnumerable<int>> GetUserQuestionAnswerReportsOfProductAsync(int productId, string userId);


        // questionAnswer
        Task AddQuestionAnswerAsync(QuestionAnswer questionAnswer);
        void UpdateQuestionAnswer(QuestionAnswer questionAnswer);
        void RemoveQuestionAnswer(QuestionAnswer questionAnswer);
        Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId);
        Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId);

        Task AddQuestionAnswerLikeAsync(QuestionAnswerLike questionAnswerLike);
        void RemoveQuestionAnswerLike(QuestionAnswerLike questionAnswerLike);
        Task<bool> IsQuestionAnswerLikeExistAsync(string userId, int questionAnswerId);

        Task AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer);
        Task<bool> IsReportQuestionAnswerExistAsync(string userId, int questionAnswerId);
        Task<ReportQuestionAnswer> GetReportQuestionAnswerAsync(string userId, int questionAnswerId);
        void RemoveReportQuestionAnswer(ReportQuestionAnswer reportQuestionAnswer);
        IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes();

        Task<bool> IsQuestionAnswerRemovableAsync(int questionAnswerId);
        Task<bool> IsUserQuestionAnswerAsync(string userId, int questionAnswerId);

        IEnumerable<QuestionAnswer> GetQuestionAnswersOfQuestion(int questionId);


        Task SaveChangesAsync();
    }
}