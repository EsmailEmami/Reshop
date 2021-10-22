using Reshop.Application.Interfaces.Conversation;
using Reshop.Domain.Interfaces.Conversation;

namespace Reshop.Application.Services.Conversation
{
    public class CommentService : ICommentService
    {
        #region constructor

        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        #endregion
    }
}