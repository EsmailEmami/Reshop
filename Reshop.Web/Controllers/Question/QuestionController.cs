using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Question;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers.Question
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #region add question

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public IActionResult AddQuestion(int productId)
        {
            if (productId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var model = new AddQuestionViewModel()
            {
                ProductId = productId,
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> AddQuestion(AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newQuestion = new Domain.Entities.Question.Question()
            {
                UserId = userId,
                QuestionDate = DateTime.Now,
                IsDelete = false,
                QuestionTitle = model.QuestionTitle,
                QuestionText = model.QuestionText,
                ProductId = model.ProductId
            };

            var res = await _questionService.AddQuestionAsync(newQuestion);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion

        #region edit question

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> EditQuestion(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var model = await _questionService.GetQuestionDataForEditAsync(questionId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> EditQuestion(EditQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var question = await _questionService.GetQuestionByIdAsync(model.QuestionId);

            if (question == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }

            question.QuestionText = model.QuestionText;
            question.QuestionTitle = model.QuestionTitle;

            var res = await _questionService.EditQuestionAsync(question);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion

        #region report question

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public IActionResult ReportQuestion(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new AddReportConversationViewModel()
            {
                Id = questionId,
                Types = _questionService.GetReportQuestionTypes()
                    .Select(c => new Tuple<int, string>(
                        c.ReportQuestionTypeId,
                        c.ReportQuestionTitle))
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> ReportQuestion(AddReportConversationViewModel model)
        {
            model.Types = _questionService.GetReportQuestionTypes()
                .Select(c => new Tuple<int, string>(
                    c.ReportQuestionTypeId,
                    c.ReportQuestionTitle));

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reportQuestion = new ReportQuestion()
            {
                UserId = userId,
                QuestionId = model.Id,
                Description = model.Description,
                ReportQuestionTypeId = model.SelectedType,
                CreateDate = DateTime.Now
            };

            var res = await _questionService.AddReportQuestionAsync(reportQuestion);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion

        #region  add question answer

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public IActionResult AddQuestionAnswer(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var model = new AddQuestionAnswerViewModel()
            {
                QuestionId = questionId,
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> AddQuestionAnswer(AddQuestionAnswerViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newQuestionAnswer = new QuestionAnswer()
            {
                UserId = userId,
                QuestionAnswerDate = DateTime.Now,
                IsDelete = false,
                QuestionId = model.QuestionId,
                AnswerText = model.AnswerText,
            };

            var res = await _questionService.AddQuestionAnswerAsync(newQuestionAnswer);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion

        #region edit question answer

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> EditQuestionAnswer(int questionAnswerId)
        {
            if (questionAnswerId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            var model = await _questionService.GetQuestionAnswerDataForEditAsync(questionAnswerId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> EditQuestionAnswer(EditQuestionAnswerViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var questionAnswer = await _questionService.GetQuestionAnswerByIdAsync(model.QuestionAnswerId);

            if (questionAnswer == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }

            questionAnswer.AnswerText = model.AnswerText;

            var res = await _questionService.EditQuestionAnswerAsync(questionAnswer);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion

        #region report question answer

        [HttpGet]
        [PermissionJs]
        [NoDirectAccess]
        public IActionResult ReportQuestionAnswer(int questionAnswerId)
        {
            if (questionAnswerId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new AddReportConversationViewModel()
            {
                Id = questionAnswerId,
                Types = _questionService.GetReportQuestionAnswerTypes()
                    .Select(c => new Tuple<int, string>(
                        c.ReportQuestionAnswerTypeId,
                        c.ReportQuestionAnswerTitle))
            };

            return View(model);
        }

        [HttpPost]
        [PermissionJs]
        [NoDirectAccess]
        public async Task<IActionResult> ReportQuestionAnswer(AddReportConversationViewModel model)
        {
            model.Types = _questionService.GetReportQuestionAnswerTypes()
                .Select(c => new Tuple<int, string>(
                    c.ReportQuestionAnswerTypeId,
                    c.ReportQuestionAnswerTitle));

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reportQuestionAnswer = new ReportQuestionAnswer()
            {
                UserId = userId,
                QuestionAnswerId = model.Id,
                Description = model.Description,
                ReportQuestionAnswerTypeId = model.SelectedType,
                CreateDate = DateTime.Now
            };

            var res = await _questionService.AddReportQuestionAnswerAsync(reportQuestionAnswer);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, null, model) });
            }
        }

        #endregion
    }
}
