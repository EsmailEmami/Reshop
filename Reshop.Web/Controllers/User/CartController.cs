using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CodeFixes;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.Product;

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
        public async Task<IActionResult> AddToCart(string userId, int productId, string shopperUserId)
        {
            var result = await _cartService.AddToCart(userId, productId, shopperUserId);


            return Json(result == ResultTypes.Successful ? new { IsSuccessful = true } : new { IsSuccessful = false });
        }

        [HttpGet]
        public IActionResult ShowCart()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveProduct(string orderDetailId, string action)
        {
            if (!await _cartService.IsOrderDetailExistAsync(orderDetailId))
                return NotFound();



            switch (action)
            {
                case "Plus":
                    await _cartService.IncreaseOrderDetailCountAsync(orderDetailId);
                    break;

                case "Minus":
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    await _cartService.ReduceOrderDetailAsync(orderDetailId, userId);
                    break;

                default:
                    return BadRequest();
            }

            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> AddToFavoriteProduct(int productId, string userId)
        {
            if (await _productService.IsProductExistAsync(productId) && await _userService.IsUserExistAsync(userId))
            {
                var favoriteProduct = new FavoriteProduct()
                {
                    UserId = userId,
                    ProductId = productId
                };

                await _productService.AddFavoriteProductAsync(favoriteProduct);
                return Redirect("/");
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> RemoveFavoriteProduct(string favoriteProductId)
        {
            var favoriteProduct = await _productService.GetFavoriteProductByIdAsync(favoriteProductId);
            if (favoriteProduct != null)
            {
                await _productService.RemoveFavoriteProductAsync(favoriteProduct);
                return Redirect("/");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
