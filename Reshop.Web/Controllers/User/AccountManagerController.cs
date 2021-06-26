using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Shopper;

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

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound();


            return View(user);
        }

        [Route("Address")]
        [HttpGet]
        public IActionResult Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addresses = _userService.GetUserAddresses(userId);

            return View(addresses);
        }

        [Route("UnFinallyOrders")]
        [HttpGet]
        public IActionResult UnFinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetNotReceivedOrders(userId));
        }

        [Route("FinallyOrders")]
        [HttpGet]
        public IActionResult FinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetReceivedOrders(userId));
        }

        [Route("Questions")]
        [HttpGet]
        public IActionResult Questions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View(_userService.GetUserQuestionsForShow(userId));
        }

        [Route("Comments")]
        [HttpGet]
        public IActionResult Comments()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_userService.GetUserCommentsForShow(userId));
        }

        [Route("FavoriteProducts")]
        [HttpGet]
        public async Task<IActionResult> FavoriteProducts(string type = "all", string sortBy = "news", int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.SortBy = sortBy.ToLower();
            ViewBag.Type = type.ToLower();

            return View(await _productService.GetUserFavoriteProductsWithPagination(userId, type, sortBy, pageId, 18));
        }

        [Route("ManageProducts")]
        [HttpGet]
        [Permission("FullManager,Shopper")]
        public async Task<IActionResult> ManageProducts(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _productService.GetShopperProductsWithPaginationAsync(userId, type, sortBy, pageId, 20));
        }

        [HttpGet]
        [Route("ShopperProductDetail/{productId}")]
        [Permission("FullManager,Shopper")]
        public async Task<IActionResult> ShopperProductDetail(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _productService.GetShopperProductAsync(productId, userId));
        }
    }
}
