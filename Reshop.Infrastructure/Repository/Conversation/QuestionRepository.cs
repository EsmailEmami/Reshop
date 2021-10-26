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

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}