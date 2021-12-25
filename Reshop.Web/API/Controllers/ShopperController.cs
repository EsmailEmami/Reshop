using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Shopper;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopperController : ControllerBase
    {
        private readonly IShopperService _shopperService;
        private readonly IDiscountService _discountService;

        public ShopperController(IShopperService shopperService, IDiscountService discountService)
        {
            _shopperService = shopperService;
            _discountService = discountService;
        }


        // this is for current shopper
        // we get shopperId from current UserId
        #region current shopper

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayCurrentShopperProductData(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
                return NotFound();

            var data = await _shopperService.GetLastThirtyDayProductDataChartAsync(productId, shopperId);

            if (data == null)
                return NotFound();


            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayCurrentShopperData()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
                return NotFound();

            var data = _shopperService.GetLastThirtyDayShopperDataChart(shopperId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        #endregion

        #region current shopper product color

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetColorsOfCurrentShopperProductData(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
                return NotFound();

            var data = await _shopperService.GetColorsOfShopperProductDataChartAsync(productId, shopperId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayCurrentShopperProductColorData(int productId, int colorId)
        {
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (string.IsNullOrEmpty(shopperId))
                return NotFound();

            string shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);


            if (string.IsNullOrEmpty(shopperProductColorId))
                return NotFound();

            var data = _shopperService.GetLastThirtyDayColorProductDataChart(shopperProductColorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        #endregion

        #region shopper product color

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayBestShoppersOfColorProduct(int productId, int colorId)
        {
            var data = _shopperService.GetLastThirtyDayBestShoppersOfColorProductChart(productId, colorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetBestShoppersOfColorProduct(int productId, int colorId)
        {
            var data = _shopperService.GetBestShoppersOfColorProductChart(productId, colorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetColorsOfShopperProductData(string shopperProductId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
                return NotFound();

            var data = _shopperService.GetColorsOfShopperProductDataChart(shopperProductId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayShopperProductColorData(string shopperProductId, int colorId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
                return NotFound();

            string shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return NotFound();

            var data = _shopperService.GetLastThirtyDayColorProductDataChart(shopperProductColorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        #endregion

        #region shopper 

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayShopperProductData(string shopperProductId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
                return NotFound();

            var data = _shopperService.GetLastThirtyDayProductDataChart(shopperProductId);

            if (data == null)
                return NotFound();


            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayShopperData(string shopperId)
        {
            if (string.IsNullOrEmpty(shopperId))
                return NotFound();

            var data = _shopperService.GetLastThirtyDayShopperDataChart(shopperId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }


        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayBestShoppersOfProduct(int productId)
        {
            var data = _shopperService.GetLastThirtyDayBestShoppersOfProductChart(productId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetBestShoppersOfProduct(int productId)
        {
            var res = _shopperService.GetBestShoppersOfProductChart(productId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        #endregion

        #region current shopper discount

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastTwentyDiscountDataOfCurrentShopperProductColor(int productId, int colorId)
        {
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return NotFound();


            string shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return NotFound();

            var data = _discountService.GetLastTwentyDiscountDataOfShopperProductColorChart(shopperProductColorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        #endregion

        #region shopper discount

        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastTwentyDiscountDataOfShopperProductColor(string shopperProductId, int colorId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
                return NotFound();


            string shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return NotFound();

            var data = _discountService.GetLastTwentyDiscountDataOfShopperProductColorChart(shopperProductColorId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        #endregion
    }
}
