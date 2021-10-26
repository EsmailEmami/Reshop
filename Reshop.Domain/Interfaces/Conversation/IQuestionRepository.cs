using Reshop.Domain.DTOs.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.Conversation
{
    public interface IQuestionRepository
    {
        IEnumerable<ShowQuestionOrCommentViewModel> GetUserQuestionsForShow(string userId);
        Task AddQuestionAsync(Question question);
        void UpdateQuestion(Question question);
        void RemoveQuestion(Question question);
        Task<int> GetQuestionsCountOfProductWithTypeAsync(int productId);
        IEnumerable<ProductQuestionsForShow> GetProductQuestionsWithPagination(int productId, int skip = 1, int take = 30, string type = "news");

        Task SaveChangesAsync();
    }
}