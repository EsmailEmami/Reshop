using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.User;
using Reshop.Infrastructure.Migrations;

namespace Reshop.Web.Controllers.User
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AccountManagerController : Controller
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public AccountManagerController(IUserService userService, ICartService cartService, IProductService productService)
        {
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
        }


        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound();


            return View(user);
        }

        [HttpGet]
        public IActionResult UserAddresses()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addresses = _userService.GetUserAddresses(userId);

            return View(addresses);
        }

        [HttpGet]
        public IActionResult UnFinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View();
        }

        [HttpGet]
        public IActionResult FinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View();
        }

        [HttpGet]
        public IActionResult PayedOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View();
        }


        [HttpGet]
        public IActionResult UserQuestions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View();
        }

        [HttpGet]
        public IActionResult UserComments()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FavoriteProducts(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View(await _productService.GetUserFavoriteProductsWithPagination(userId, type, sortBy, pageId, 18));
        }

    }
}
