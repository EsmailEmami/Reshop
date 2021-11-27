using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using System.Security.Claims;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace Reshop.Web.Controllers.User
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs]
        public async Task<IActionResult> AddToCart(string shopperProductColorId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _cartService.AddToCart(userId, shopperProductColorId);

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
        public IActionResult ShowCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = _cartService.GetUserOpenOrderForShowCart(userId);

            if (model == null)
                return BadRequest();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveProduct(string trackingCode, string actionType)
        {
            if (string.IsNullOrEmpty(trackingCode))
                return BadRequest();

            var orderDetailId = await _cartService.GetOrderDetailIdByTrackingCodeAsync(trackingCode);

            if (string.IsNullOrEmpty(orderDetailId))
                return NotFound();

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
        public async Task<IActionResult> RemoveOrderDetail(string trackingCode)
        {
            if (string.IsNullOrEmpty(trackingCode))
                return BadRequest();

            var orderDetailId = await _cartService.GetOrderDetailIdByTrackingCodeAsync(trackingCode);

            if (string.IsNullOrEmpty(orderDetailId))
                return NotFound();

            var result = await _cartService.RemoveOrderDetailAsync(orderDetailId);

            if (result == ResultTypes.Failed) return BadRequest();


            return RedirectToAction(nameof(ShowCart));
        }

        [HttpGet]
        public async Task<IActionResult> Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _userService.GetUserAddressesForShowAsync(userId));
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> Address(string addressId)
        {
            if (addressId == null)
            {
                return Json(new { isValid = false, errorType = "warning", errorText = "لطفا نشانی خود را انتخاب کنید." });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _userService.IsUserAddressExistAsync(addressId, userId))
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }


            var orderId = await _cartService.GetUserOpenCartOrderIdAsync(userId);

            if (string.IsNullOrEmpty(orderId))
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }

            var res = await _cartService.AddAddressToOrderAsync(orderId, addressId);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, errorType = "success", errorText = "نشانی شما با موفقیت انتخاب شد.", returnUrl = "/Payment" });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        [HttpGet]
        [Route("Payment")]
        public async Task<IActionResult> Payment()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _cartService.GetOpenOrderForPaymentAsync(userId);

            if (order == null)
                return NotFound();

            var payment = new Payment((int)order.Item2);

            var res = await payment.PaymentRequest($"خرید فاکتور {order.Item3}", "https://localhost:44312/OnlinePayment/" + order.Item1, "esmaeilemami84@gmail.com", "09903669556");

            if (res.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Authority);
            }

            return BadRequest();
        }
    }
}
