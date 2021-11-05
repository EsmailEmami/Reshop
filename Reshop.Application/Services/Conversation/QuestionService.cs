using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;
using Reshop.Domain.Interfaces.Conversation;
using System;
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

        public async Task<bool> IsQuestionRemovableAsync(int questionId) =>
            await _questionRepository.IsQuestionRemovableAsync(questionId);

        public async Task<bool> IsUserQuestionAsync(string userId, int questionId) =>
            await _questionRepository.IsUserQuestionAsync(userId, questionId);

        public async Task<Tuple<IEnumerable<ProductQuestionsForShow>, int, int>> GetProductQuestionsWithPaginationAsync(int productId, int pageId = 1, int take = 30, string type = "news", string filter = "")
        {
            var questionsCount = await _questionRepository.GetQuestionsCountOfProductAsync(productId);

            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int totalPages = (int)Math.Ceiling(1.0 * questionsCount / take);

            var questions = await _questionRepository.GetProductQuestionsWithPaginationAsync(productId, skip, take, type.FixedText(), filter);

            return new Tuple<IEnumerable<ProductQuestionsForShow>, int, int>(questions, pageId, totalPages);
        }

        public async Task<ResultTypes> DeleteQuestionAsync(int questionId, string description)
        {
            try
            {
                if (!await _questionRepository.IsQuestionRemovableAsync(questionId))
                    return ResultTypes.Failed;

                var question = await _questionRepository.GetQuestionByIdAsync(questionId);

                if (question == null)
                    return ResultTypes.Failed;

                var questionAnswers = _questionRepository.GetQuestionAnswersOfQuestion(question.QuestionId);

                foreach (var questionAnswer in questionAnswers)
                {
                    questionAnswer.IsDelete = true;
                    questionAnswer.DeleteDescription = "سوال پرسیده شده حذف گردید.";

                    _questionRepository.UpdateQuestionAnswer(questionAnswer);
                }

                question.IsDelete = true;
                question.DeleteDescription = description;

                _questionRepository.UpdateQuestion(question);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<QuestionAndAnswerResultTypes> LikeQuestionAsync(string userId, int questionId)
        {
            try
            {
                if (await _questionRepository.IsQuestionLikeExistAsync(userId, questionId))
                {
                    var questionLike = await _questionRepository.GetQuestionLikeAsync(userId, questionId);

                    if (questionLike == null)
                        return QuestionAndAnswerResultTypes.Failed;

                    _questionRepository.RemoveQuestionLike(questionLike);

                    await _questionRepository.SaveChangesAsync();

                    return QuestionAndAnswerResultTypes.Deleted;
                }
                else
                {
                    var questionLike = new QuestionLike()
                    {
                        UserId = userId,
                        QuestionId = questionId
                    };

                    await _questionRepository.AddQuestionLikeAsync(questionLike);

                    await _questionRepository.SaveChangesAsync();

                    return QuestionAndAnswerResultTypes.Added;
                }
            }
            catch
            {
                return QuestionAndAnswerResultTypes.Failed;
            }
        }

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

        public async Task<ResultTypes> RemoveReportQuestionAsync(string userId, int questionId)
        {
            try
            {
                var report = await _questionRepository.GetReportQuestionAsync(userId, questionId);

                if (report == null)
                    return ResultTypes.Failed;

                _questionRepository.RemoveReportQuestion(report);

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

        public async Task<IEnumerable<int>> GetUserQuestionLikesOfProductAsync(int productId, string userId) =>
            await _questionRepository.GetUserQuestionLikesOfProductAsync(productId, userId);

        public async Task<IEnumerable<int>> GetUserQuestionReportsOfProductAsync(int productId, string userId) =>
            await _questionRepository.GetUserQuestionReportsOfProductAsync(productId, userId);

        public async Task<IEnumerable<int>> GetUserQuestionAnswerLikesOfProductAsync(int productId, string userId) =>
            await _questionRepository.GetUserQuestionAnswerLikesOfProductAsync(productId, userId);

        public async Task<IEnumerable<int>> GetUserQuestionAnswerReportsOfProductAsync(int productId, string userId) =>
            await _questionRepository.GetUserQuestionAnswerReportsOfProductAsync(productId, userId);

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

        public async Task<QuestionAndAnswerResultTypes> LikeQuestionAnswerAsync(QuestionAnswerLike questionAnswerLike)
        {
            try
            {
                if (await _questionRepository.IsQuestionAnswerLikeExistAsync(questionAnswerLike.UserId, questionAnswerLike.QuestionAnswerId))
                {
                    _questionRepository.RemoveQuestionAnswerLike(questionAnswerLike);

                    await _questionRepository.SaveChangesAsync();

                    return QuestionAndAnswerResultTypes.Deleted;
                }
                else
                {
                    await _questionRepository.AddQuestionAnswerLikeAsync(questionAnswerLike);

                    await _questionRepository.SaveChangesAsync();

                    return QuestionAndAnswerResultTypes.Added;
                }
            }
            catch
            {
                return QuestionAndAnswerResultTypes.Failed;
            }
        }

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

        public async Task<ResultTypes> RemoveReportQuestionAnswerAsync(string userId, int questionAnswerId)
        {
            try
            {
                var report = await _questionRepository.GetReportQuestionAnswerAsync(userId, questionAnswerId);

                if (report == null)
                    return ResultTypes.Failed;

                _questionRepository.RemoveReportQuestionAnswer(report);

                await _questionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsQuestionAnswerRemovableAsync(int questionAnswerId) =>
            await _questionRepository.IsQuestionAnswerRemovableAsync(questionAnswerId);

        public async Task<bool> IsUserQuestionAnswerAsync(string userId, int questionAnswerId) =>
            await _questionRepository.IsUserQuestionAnswerAsync(userId, questionAnswerId);

        public async Task<ResultTypes> DeleteQuestionAnswerAsync(int questionAnswerId, string description)
        {
            try
            {
                if (!await _questionRepository.IsQuestionAnswerRemovableAsync(questionAnswerId))
                    return ResultTypes.Failed;

                var questionAnswer = await _questionRepository.GetQuestionAnswerByIdAsync(questionAnswerId);

                if (questionAnswer == null)
                    return ResultTypes.Failed;

                questionAnswer.IsDelete = true;
                questionAnswer.DeleteDescription = description;

                _questionRepository.UpdateQuestionAnswer(questionAnswer);

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