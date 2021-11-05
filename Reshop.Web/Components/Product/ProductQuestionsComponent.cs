﻿using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Conversation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Components.Product
{
    public class ProductQuestionsComponent : ViewComponent
    {
        private readonly IQuestionService _questionService;

        public ProductQuestionsComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // type = news,buyers,best
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageId = 1, string type = "news")
        {
            var questions = await _questionService.GetProductQuestionsWithPaginationAsync(productId, pageId, 25);

            ViewBag.SelectedType = type;
            ViewBag.ProductId = productId;


            IEnumerable<int> userQuestionLikes = new List<int>();
            IEnumerable<int> reportedQuestions = new List<int>();
            IEnumerable<int> userQuestionAnswerLikes = new List<int>();
            IEnumerable<int> reportedQuestionAnswers = new List<int>();


            if (User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                userQuestionLikes = await _questionService.GetUserQuestionLikesOfProductAsync(productId, userId);
                reportedQuestions = await _questionService.GetUserQuestionReportsOfProductAsync(productId, userId);
                userQuestionAnswerLikes = await _questionService.GetUserQuestionAnswerLikesOfProductAsync(productId, userId);
                reportedQuestionAnswers = await _questionService.GetUserQuestionAnswerReportsOfProductAsync(productId, userId);

            }

            ViewBag.UserQuestionLikes = userQuestionLikes;
            ViewBag.ReportedQuestions = reportedQuestions;

            ViewBag.UserQuestionAnswerLikes = userQuestionAnswerLikes;
            ViewBag.ReportedQuestionAnswers = reportedQuestionAnswers;

            return View("/Views/Shared/Components/Product/ProductQuestions.cshtml", questions);
        }
    }
}
