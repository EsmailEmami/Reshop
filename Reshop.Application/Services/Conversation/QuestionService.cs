using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;
using Reshop.Domain.Interfaces.Conversation;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<ResultTypes> AddQuestionAsync(Question question)
        {
            try
            {
                await _questionRepository.AddQuestionAsync(question);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditQuestionAsync(Question question)
        {
            try
            {
                _questionRepository.UpdateQuestion(question);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId) =>
            await _questionRepository.GetQuestionByIdAsync(questionId);

        public async Task<EditQuestionViewModel> GetQuestionDataForEditAsync(int questionId) =>
            await _questionRepository.GetQuestionDataForEditAsync(questionId);

        public async Task<ResultTypes> AddReportQuestionAsync(ReportQuestion reportQuestion)
        {
            try
            {
                if (await _questionRepository.IsReportQuestionExistAsync(reportQuestion.UserId, reportQuestion.QuestionId))
                    return ResultTypes.Failed;

                await _questionRepository.AddReportQuestionAsync(reportQuestion);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<ReportQuestionType> GetReportQuestionTypes() =>
            _questionRepository.GetReportQuestionTypes();

        public async Task<ResultTypes> AddQuestionAnswerAsync(QuestionAnswer questionAnswer)
        {
            try
            {
                await _questionRepository.AddQuestionAnswerAsync(questionAnswer);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditQuestionAnswerAsync(QuestionAnswer questionAnswer)
        {
            try
            {
                _questionRepository.UpdateQuestionAnswer(questionAnswer);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<QuestionAnswer> GetQuestionAnswerByIdAsync(int questionAnswerId) =>
            await _questionRepository.GetQuestionAnswerByIdAsync(questionAnswerId);

        public async Task<EditQuestionAnswerViewModel> GetQuestionAnswerDataForEditAsync(int questionAnswerId) =>
            await _questionRepository.GetQuestionAnswerDataForEditAsync(questionAnswerId);

        public async Task<ResultTypes> AddReportQuestionAnswerAsync(ReportQuestionAnswer reportQuestionAnswer)
        {
            try
            {
                if (await _questionRepository.IsReportQuestionExistAsync(reportQuestionAnswer.UserId, reportQuestionAnswer.QuestionAnswerId))
                    return ResultTypes.Failed;

                await _questionRepository.AddReportQuestionAnswerAsync(reportQuestionAnswer);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IEnumerable<ReportQuestionAnswerType> GetReportQuestionAnswerTypes() =>
            _questionRepository.GetReportQuestionAnswerTypes();
    }
}