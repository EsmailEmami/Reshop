using Reshop.Domain.Interfaces.Conversation;
using Reshop.Infrastructure.Context;

namespace Reshop.Infrastructure.Repository.Conversation
{
    public class CommentRepository : ICommentRepository
    {
        #region constructor 

        private readonly ReshopDbContext _context;

        public CommentRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion
    }
}