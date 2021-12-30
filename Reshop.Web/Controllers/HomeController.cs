using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Interfaces.User;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartService _cartService;

        public HomeController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Route("Error")]
        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode = 0)
        {
            return View(statusCode);
        }

        [Route("ErrorJs")]
        [Route("ErrorJs/{statusCode}")]
        [NoDirectAccess]
        public IActionResult ErrorJs(int statusCode = 0)
        {
            return View("ErrorJs", statusCode);
        }


        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("FQS")]
        public IActionResult FrequentlyQuestions()
        {
            return View();
        }

        [Route("Guide")]
        public IActionResult Guide()
        {
            return View();
        }

        [Route("OnlinePayment/{paymentId}")]
        public async Task<IActionResult> OnlinePayment(string paymentId)
        {

            if (!HttpContext.Request.IsHttps &&
                !HttpContext.Request.Headers["Referer"].ToString().ToLower().StartsWith("https://sandbox.zarinpal.com/"))
            {
                return BadRequest();
            }


            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var order = await _cartService.GetOpenOrderForPaymentAsync(userId);

                if (order == null)
                    return NotFound();


                var payment = new ZarinpalSandbox.Payment((int)order.Item2);

                var res = payment.Verification(authority).Result;

                ViewBag.IsSuccess = false;

                if (res.Status == 100)
                {
                    ViewBag.IsSuccess = true;

                    await _cartService.MakeFinalTheOrder(order.Item1);
                }
            }

            return View();
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        [Route("file_upload")]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null;

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "productDescriptionImages",
                "images");

            string imageName = await ImageConvertor.CreateNewImage(upload, path);

            var url = $"/images/productDescriptionImages/images/{imageName}";


            return Json(new { uploaded = true, url });
        }
    }
}
