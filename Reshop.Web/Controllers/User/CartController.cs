using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
    [AutoValidateAntiforgeryToken]
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
        [ValidateAntiForgeryToken]
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
            return View(_cartService.GetUserOpenOrderForShowCart(userId));
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
        public IActionResult Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string addressId = _cartService.GetOpenOrderAddressId(userId);

            if (!string.IsNullOrEmpty(addressId))
            {
                ViewData["SelectedAddress"] = addressId;
            }
            else
            {
                ViewData["SelectedAddress"] = "none";
            }



            return View(_userService.GetUserAddresses(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address(string addressId)
        {
            if (addressId == null)
            {
                return Json(new { isValid = false, isNull = true });
            }


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _userService.IsUserAddressExistAsync(addressId, userId))
            {
                return Json(new { isValid = false, isNull = false });
            }


            var order = await _cartService.GetUserOpenOrderAsync(userId);

            if (order is null)
            {
                return Json(new { isValid = false, isNull = false });
            }

            order.AddressId = addressId;
            await _cartService.EditOrderAsync(order);

            return Json(new { isValid = true, isNull = false });
        }

        [HttpGet]
        [Route("Payment")]
        public async Task<IActionResult> Payment()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _cartService.GetUserOpenOrderAsync(userId);

            if (order == null) return NotFound();

            if (string.IsNullOrEmpty(order.AddressId))
            {
                return BadRequest();
            }



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
