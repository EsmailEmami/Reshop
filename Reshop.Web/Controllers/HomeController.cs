using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.User;

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

        [Route("FrequentlyQuestions")]
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

                var order = await _cartService.GetOrderByIdAsync(paymentId);

                if (order is null)
                {
                    return NotFound();
                }

                if (order.IsPayed || order.IsReceived)
                {
                    return BadRequest();
                }

                var payment = new ZarinpalSandbox.Payment((int)order.Sum);
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    await _cartService.MakeFinalTheOrder(order);
                }
            }

            return View();
        }
    }
}
