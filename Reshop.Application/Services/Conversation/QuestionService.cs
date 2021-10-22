using Reshop.Application.Interfaces.Conversation;
using Reshop.Domain.Interfaces.Conversation;

namespace Reshop.Application.Services.Conversation
{
    public class QuestionService : IQuestionService
    {
        #region constructor

        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        #endregion
    }
}