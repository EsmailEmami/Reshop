using Reshop.Domain.Interfaces.Conversation;
using Reshop.Infrastructure.Context;

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
    }
}