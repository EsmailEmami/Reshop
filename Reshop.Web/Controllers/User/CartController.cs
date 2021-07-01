﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using System.Security.Claims;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace Reshop.Web.Controllers.User
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IProductService productService, IUserService userService)
        {
            _cartService = cartService;
            _productService = productService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string shopperUserId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _cartService.AddToCart(userId, productId, shopperUserId);

            if (result == ResultTypes.Successful)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        [Route("Cart")]
        public async Task<IActionResult> ShowCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _cartService.GetUserOpenOrderForShowCartAsync(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveProduct(string orderDetailId, string actionType)
        {
            if (!await _cartService.IsOrderDetailExistAsync(orderDetailId))
                return NotFound();



            switch (actionType)
            {
                case "Plus":
                    await _cartService.IncreaseOrderDetailCountAsync(orderDetailId);
                    break;

                case "Minus":


                    await _cartService.ReduceOrderDetailAsync(orderDetailId);
                    break;

                default:
                    return BadRequest();
            }

            return RedirectToAction(nameof(ShowCart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveOrderDetail(string orderDetailId)
        {
            var result = await _cartService.RemoveOrderDetailAsync(orderDetailId);

            if (result == ResultTypes.Failed) return BadRequest();


            return RedirectToAction(nameof(ShowCart));
        }

        [HttpGet]
        public async Task<IActionResult> Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_userService.GetUserAddresses(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Address(string addressId)
        {
            //TODO select order address


            return RedirectToAction(nameof(Payment));
        }

        [HttpGet]
        [Route("Payment")]
        public async Task<IActionResult> Payment()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _cartService.GetUserOpenOrderAsync(userId);

            if (order == null) return NotFound();

            var payment = new Payment((int)order.Sum);

            var res = await payment.PaymentRequest($"خرید فاکتور {order.TrackingCode}", "https://localhost:44312/OnlinePayment/" + order.OrderId, "esmaeilemami84@gmail.com", "09903669556");

            if (res.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Authority);
            }

            return BadRequest();
        }
    }
}
