using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Question;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.Conversation
{
    public class QuestionRepository : IQuestionRepository
    {
        #region constructor 

        private readonly ReshopDbContext _context;

        public QuestionRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        public IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId) =>
            _context.Questions.Where(c => c.UserId == userId)
                .Select(c => new ShowQuestionOrCommentViewModel()
                {
                    QuestionOrCommentId = c.QuestionId,
                    QuestionOrCommentTitle = c.QuestionTitle,
                    SentDate = c.QuestionDate
                });

        public async Task AddQuestionAsync(Question question) =>
            await _context.Questions.AddAsync(question);

        public void UpdateQuestion(Question question) =>
            _context.Questions.Update(question);

        public void RemoveQuestion(Question question) =>
            _context.Questions.Remove(question);

        public async Task<Question> GetQuestionByIdAsync(int questionId) =>
            await _context.Questions.FindAsync(questionId);

        public async Task<EditQuestionViewModel> GetQuestionDataForEditAsync(int questionId) =>
            await _context.Questions
                .Where(c => c.QuestionId == questionId)
                .Select(c => new EditQuestionViewModel()
                {
                    QuestionId = c.QuestionId,
                    QuestionTitle = c.QuestionTitle,
                    QuestionText = c.QuestionText,
                }).SingleOrDefaultAsync();

        public async Task<int> GetQuestionsCountOfProductWithTypeAsync(int productId) =>
            await _context.Questions
                .Where(c => c.ProductId == productId)
                .CountAsync();

        public IEnumerable<ProductQuestionsForShow> GetProductQuestionsWithPagination(int productId, int skip = 1, int take = 30, string type = "news")
        {
            IQueryable<Question> questions = _context.Questions
                .Where(c => c.ProductId == productId && !c.IsDelete);

            questions = type switch
            {
                "news" => questions.OrderByDescending(c => c.QuestionDate),
                "best" => questions.OrderByDescending(c => c.QuestionLikes.Count),
                _ => questions.OrderByDescending(c => c.QuestionDate)
            };

            questions = questions.Skip(skip).Take(take);

            return questions.Select(c => new ProductQuestionsForShow()
            {
                QuestionId = c.QuestionId,
                FullName = c.User.FullName,
                Image = c.User.UserAvatar,
                QuestionDate = c.QuestionDate,
                QuestionTitle = c.QuestionTitle,
                AnswersCount = c.QuestionAnswers.Count
            });
        }

        public async Task AddReportQuestionAsync(ReportQuestion reportQuestion) =>
            await _context.QuestionReports.AddAsync(reportQuestion);

        public async Task<bool> IsReportQuestionExistAsync(string userId, int questionId) =>
            await _context.QuestionReports.AnyAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public IEnumerable<ReportQuestionType> GetReportQuestionTypes() =>
             _context.ReportQuestionTypes;

        public async Task AddQuestionAnswerAsync(QuestionAnswer questionAnswer) =>
            await _context.QuestionAnswers.AddAsync(questionAnswer);

        public void UpdateQuestionAnswer(QuestionAnswer questionAnswer) =>
            _context.QuestionAnswers.Update(questionAnswer);

        public void RemoveQuestionAnswer(QuestionAnswer questionAnswer) =>
            _context.QuestionAnswers.Remove(questionAnswer);

        public async Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId) =>
        await _context.QuestionAnswers.FindAsync(questionAnswerId);

        public async Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId) =>
            await _context.QuestionAnswers
                .Where(c => c.QuestionAnswerId == questionAnswerId)
                .Select(c => new EditQuestionAnswerViewModel()
                {
                    QuestionAnswerId = c.QuestionAnswerId,
                    AnswerText = c.AnswerText,
                }).SingleOrDefaultAsync();

        public async Task AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer) =>
            await _context.QuestionAnswerReports.AddAsync(reportQuestionAnswer);


        public async Task<bool> IsReportQuestionAnswerExistAsync(string userId, int questionAnswerId) =>
            await _context.QuestionAnswerReports.AnyAsync(c => c.UserId == userId && c.QuestionAnswerId == questionAnswerId);

        public IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes() =>
            _context.ReportQuestionAnswerTypes;

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}