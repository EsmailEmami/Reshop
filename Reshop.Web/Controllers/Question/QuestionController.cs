using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.User;
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
    // most methods of this controller return json then we used PermissionJs attr
    [PermissionJs]
    // methods must not load in route 
    [NoDirectAccess]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [AllowAnonymous]
        public IActionResult ProductQuestionsList(int productId, int pageId, string type, string filter)
        {
            return ViewComponent("ProductQuestionsComponent", new { productId, pageId, type, filter });
        }



        #region add question

        [HttpPost]
        public async Task<IActionResult> AddQuestion(AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return new ObjectResult(new
                {
                    isValid = false,
                    html = RenderViewToString.RenderRazorViewToString(this, "Question/_NewQuestion", model)
                });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newQuestion = new Domain.Entities.Question.Question()
            {
                UserId = userId,
                ProductId = model.ProductId,
                QuestionDate = DateTime.Now,
                IsDelete = false,
                DeleteDescription = "NULL",
                QuestionTitle = model.QuestionTitle,
                QuestionText = model.QuestionText,
            };

            var res = await _questionService.AddQuestionAsync(newQuestion);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "Question/_NewQuestion", model) });
            }
        }

        #endregion

        #region edit question

        [HttpGet]
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
        public async Task<IActionResult> EditQuestion(EditQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var question = await _questionService.GetQuestionByIdAsync(model.QuestionId);

            if (question == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
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

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region remove question

        [HttpGet]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _questionService.IsQuestionRemovableAsync(questionId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            if (!await _questionService.IsUserQuestionAsync(userId, questionId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = new DeleteConversationViewModel()
            {
                Id = questionId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(DeleteConversationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _questionService.IsUserQuestionAsync(userId, model.Id))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var res = await _questionService.DeleteQuestionAsync(model.Id, model.DeleteDescription);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region report question

        [HttpGet]
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
        public async Task<IActionResult> ReportQuestion(AddReportConversationViewModel model)
        {
            model.Types = _questionService.GetReportQuestionTypes()
                .Select(c => new Tuple<int, string>(
                    c.ReportQuestionTypeId,
                    c.ReportQuestionTitle));

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
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

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region remove report question

        [HttpPost]
        public async Task<IActionResult> RemoveReportQuestion(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _questionService.RemoveReportQuestionAsync(userId, questionId);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        #endregion

        #region like question

        [HttpPost]
        public async Task<IActionResult> LikeQuestion(int questionId)
        {
            if (questionId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _questionService.LikeQuestionAsync(userId, questionId);

            return res switch
            {
                QuestionAndAnswerResultTypes.Added => Json(new
                {
                    isValid = true,
                    where = "Added",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت ثبت شد."
                }),
                QuestionAndAnswerResultTypes.Deleted => Json(new
                {
                    isValid = true,
                    where = "Deleted",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت حذف شد."
                }),
                QuestionAndAnswerResultTypes.Failed => Json(new
                {
                    isValid = false,
                    where = "Failed",
                    errorType = "danger",
                    errorText = "متاسفانه مشکلی پیش آمده است."
                }),
                _ => Json(new { isValid = false, errorType = "danger", errorText = "متاسفانه مشکلی پیش آمده است." })
            };
        }

        #endregion

        #region  add question answer

        [HttpGet]
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
        public async Task<IActionResult> AddQuestionAnswer(AddQuestionAnswerViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

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
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region edit question answer

        [HttpGet]
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
        public async Task<IActionResult> EditQuestionAnswer(EditQuestionAnswerViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var questionAnswer = await _questionService.GetQuestionAnswerByIdAsync(model.QuestionAnswerId);

            if (questionAnswer == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش سوال شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
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

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region report question answer

        [HttpGet]
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
        public async Task<IActionResult> ReportQuestionAnswer(AddReportConversationViewModel model)
        {
            model.Types = _questionService.GetReportQuestionAnswerTypes()
                .Select(c => new Tuple<int, string>(
                    c.ReportQuestionAnswerTypeId,
                    c.ReportQuestionAnswerTitle));

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (model.SelectedType == 0)
            {
                ModelState.AddModelError("Types", "لطفا عنوان گزارش را انتخاب کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
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

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region like question answer

        [HttpPost]
        public async Task<IActionResult> LikeQuestionAnswer(int questionAnswerId)
        {
            if (questionAnswerId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var like = new QuestionAnswerLike()
            {
                QuestionAnswerId = questionAnswerId,
                UserId = userId
            };

            var res = await _questionService.LikeQuestionAnswerAsync(like);

            return res switch
            {
                QuestionAndAnswerResultTypes.Added => Json(new
                {
                    isValid = true,
                    where = "Added",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت ثبت شد."
                }),
                QuestionAndAnswerResultTypes.Deleted => Json(new
                {
                    isValid = true,
                    where = "Deleted",
                    errorType = "success",
                    errorText = "بازخورد شما با موفقیت حذف شد."
                }),
                QuestionAndAnswerResultTypes.Failed => Json(new
                {
                    isValid = false,
                    where = "Failed",
                    errorType = "danger",
                    errorText = "متاسفانه مشکلی پیش آمده است."
                }),
                _ => Json(new { isValid = false, errorType = "danger", errorText = "متاسفانه مشکلی پیش آمده است." })
            };
        }

        #endregion

        #region remove report question

        [HttpPost]
        public async Task<IActionResult> RemoveReportQuestionAnswer(int questionAnswerId)
        {
            if (questionAnswerId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _questionService.RemoveReportQuestionAnswerAsync(userId, questionAnswerId);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        #endregion

        #region remove question

        [HttpGet]
        public async Task<IActionResult> DeleteQuestionAnswer(int questionAnswerId)
        {
            if (questionAnswerId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _questionService.IsQuestionAnswerRemovableAsync(questionAnswerId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


            if (!await _questionService.IsUserQuestionAnswerAsync(userId, questionAnswerId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = new DeleteConversationViewModel()
            {
                Id = questionAnswerId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestionAnswer(DeleteConversationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _questionService.IsUserQuestionAnswerAsync(userId, model.Id))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var res = await _questionService.DeleteQuestionAnswerAsync(model.Id, model.DeleteDescription);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت گزارش شما به مشکلی غیر منتظره برخوردیم! لطفا دوباره تلاش کنید.");

                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion
    }
}