using Microsoft.EntityFrameworkCore;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Question;
using Reshop.Domain.Interfaces.Conversation;
using Reshop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;

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

        public async Task<bool> IsQuestionRemovableAsync(int questionId) =>
            await _context.Questions.AnyAsync(c =>
                c.QuestionId == questionId && c.QuestionDate >= DateTime.Now.AddHours(-3));

        public async Task<int> GetQuestionsCountOfProductAsync(int productId) =>
            await _context.Questions
                .Where(c => c.ProductId == productId)
                .CountAsync();

        public async Task<IEnumerable<ProductQuestionsForShow>> GetProductQuestionsWithPaginationAsync(int productId, int skip = 1, int take = 30, string type = "news",string filter = "")
        {
            IQueryable<Question> questions = _context.Questions
                .Where(c => c.ProductId == productId && !c.IsDelete);

            questions = type switch
            {
                "news" => questions.OrderByDescending(c => c.QuestionDate),
                "best" => questions.OrderByDescending(c => c.QuestionLikes.Count),
                _ => questions.OrderByDescending(c => c.QuestionDate)
            };

            if (!string.IsNullOrEmpty(filter))
            {
                questions = questions.Where(c => c.QuestionText.Contains(filter) || c.QuestionTitle.Contains(filter));
            }

            questions = questions.Skip(skip).Take(take);

            return await questions.Select(c => new ProductQuestionsForShow()
            {
                QuestionId = c.QuestionId,
                FullName = c.User.FullName,
                Image = c.User.UserAvatar,
                QuestionDate = c.QuestionDate,
                QuestionTitle = c.QuestionTitle,
                QuestionText = c.QuestionText,
                Likes = c.QuestionLikes.Count,
                Answers = c.QuestionAnswers.Select(a => new QuestionAnswersForShow()
                {
                    QuestionAnswerId = a.QuestionAnswerId,
                    AnswerDate = a.QuestionAnswerDate,
                    AnswerText = a.AnswerText,
                    Likes = a.QuestionAnswerLikes.Count
                })
            }).ToListAsync();
        }

        public async Task<bool> IsUserQuestionAsync(string userId, int questionId) =>
            await _context.Questions.AnyAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public async Task<QuestionLike> GetQuestionLikeAsync(string userId, int questionId) =>
            await _context.QuestionLikes.SingleOrDefaultAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public async Task AddQuestionLikeAsync(QuestionLike questionLike) =>
            await _context.QuestionLikes.AddAsync(questionLike);

        public void RemoveQuestionLike(QuestionLike questionLike) =>
             _context.QuestionLikes.Remove(questionLike);

        public async Task<bool> IsQuestionLikeExistAsync(string userId, int questionId) =>
            await _context.QuestionLikes.AnyAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public async Task AddReportQuestionAsync(ReportQuestion reportQuestion) =>
            await _context.QuestionReports.AddAsync(reportQuestion);

        public async Task<bool> IsReportQuestionExistAsync(string userId, int questionId) =>
            await _context.QuestionReports.AnyAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public async Task<ReportQuestion> GetReportQuestionAsync(string userId, int questionId) =>
            await _context.QuestionReports
                .SingleOrDefaultAsync(c => c.UserId == userId && c.QuestionId == questionId);

        public void RemoveReportQuestion(ReportQuestion reportQuestion) =>
            _context.QuestionReports.Remove(reportQuestion);

        public IEnumerable<ReportQuestionType> GetReportQuestionTypes() =>
             _context.ReportQuestionTypes;

        public async Task<IEnumerable<int>> GetUserQuestionLikesOfProductAsync(int productId, string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.QuestionLikes)
                .Where(c => c.Question.ProductId == productId)
                .Select(c => c.QuestionId).ToListAsync();

        public async Task<IEnumerable<int>> GetUserQuestionReportsOfProductAsync(int productId, string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.QuestionReports)
                .Where(c => c.Question.ProductId == productId)
                .Select(c => c.QuestionId).ToListAsync();

        public async Task<IEnumerable<int>> GetUserQuestionAnswerLikesOfProductAsync(int productId, string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.QuestionAnswerLikes)
                .Where(c => c.QuestionAnswer.Question.ProductId == productId)
                .Select(c => c.QuestionAnswerId).ToListAsync();

        public async Task<IEnumerable<int>> GetUserQuestionAnswerReportsOfProductAsync(int productId, string userId) =>
            await _context.Users.Where(c => c.UserId == userId)
                .SelectMany(c => c.QuestionAnswerReports)
                .Where(c => c.QuestionAnswer.Question.ProductId == productId)
                .Select(c => c.QuestionAnswerId).ToListAsync();

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

        public async Task AddQuestionAnswerLikeAsync(QuestionAnswerLike questionAnswerLike) =>
            await _context.QuestionAnswerLikes.AddAsync(questionAnswerLike);

        public void RemoveQuestionAnswerLike(QuestionAnswerLike questionAnswerLike) =>
            _context.QuestionAnswerLikes.Remove(questionAnswerLike);

        public async Task<bool> IsQuestionAnswerLikeExistAsync(string userId, int questionAnswerId) =>
            await _context.QuestionAnswerLikes.AnyAsync(c => c.UserId == userId && c.QuestionAnswerId == questionAnswerId);

        public async Task AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer) =>
            await _context.QuestionAnswerReports.AddAsync(reportQuestionAnswer);

        public async Task<bool> IsReportQuestionAnswerExistAsync(string userId, int questionAnswerId) =>
            await _context.QuestionAnswerReports.AnyAsync(c => c.UserId == userId && c.QuestionAnswerId == questionAnswerId);

        public async Task<ReportQuestionAnswer> GetReportQuestionAnswerAsync(string userId, int questionAnswerId) =>
            await _context.QuestionAnswerReports
                .SingleOrDefaultAsync(c => c.UserId == userId && c.QuestionAnswerId == questionAnswerId);

        public void RemoveReportQuestionAnswer(ReportQuestionAnswer reportQuestionAnswer) =>
            _context.QuestionAnswerReports.Remove(reportQuestionAnswer);

        public IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes() =>
            _context.ReportQuestionAnswerTypes;

        public async Task<bool> IsQuestionAnswerRemovableAsync(int questionAnswerId) =>
            await _context.QuestionAnswers.AnyAsync(c =>
                c.QuestionAnswerId == questionAnswerId && c.QuestionAnswerDate >= DateTime.Now.AddHours(-3));

        public async Task<bool> IsUserQuestionAnswerAsync(string userId, int questionAnswerId) =>
            await _context.QuestionAnswers.AnyAsync(c => c.UserId == userId && c.QuestionAnswerId == questionAnswerId);

        public IEnumerable<QuestionAnswer> GetQuestionAnswersOfQuestion(int questionId) =>
             _context.Questions.Where(c => c.QuestionId == questionId)
                .SelectMany(c => c.QuestionAnswers);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}