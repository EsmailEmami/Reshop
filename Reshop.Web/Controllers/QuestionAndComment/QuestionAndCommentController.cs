using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Controllers.QuestionAndComment
{
    public class QuestionAndCommentController : Controller
    {
        private readonly IProductService _productService;

        public QuestionAndCommentController(IProductService productService)
        {
            _productService = productService;
        }


        [Route("Questions/{productId}/{pageId}")]
        public IActionResult ProductQuestions(int productId,int pageId = 1)
        {
            return View(_productService.GetProductQuestions(productId));
        }
    }
}
