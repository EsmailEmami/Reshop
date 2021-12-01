using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Shopper;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Security.Attribute;

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


        [HttpGet("GetLastThirtyDayProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayProductDataChart(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId is null)
            {
                return NotFound();
            }

            var res = await _shopperService.GetLastThirtyDayProductDataChartAsync(productId, shopperId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        [HttpGet("GetLastThirtyDayBestShoppersOfProduct/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayBestShoppersOfProductChart(int productId)
        {
            var res = _shopperService.GetLastThirtyDayBestShoppersOfProductChart(productId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        [HttpGet("GetBestShoppersOfProduct/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetBestShoppersOfProductChart(int productId)
        {
            var res = _shopperService.GetBestShoppersOfProductChart(productId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        [HttpGet("GetColorsOfShopperProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetColorsOfShopperProductDataChart(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId is null)
            {
                return NotFound();
            }

            var res = await _shopperService.GetColorsOfShopperProductDataChartAsync(productId, shopperId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        // this is for shopper and shopperId come from shopperUserId automatically
        //color
        [HttpGet("GetLastThirtyDayColorProductData/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastThirtyDayColorProductDataChart(int productId, int colorId)
        {
            string shopperId =
                await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
            {
                return NotFound();
            }

            string shopperProductColorId =
                await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
            {
                return NotFound();
            }

            var res = _shopperService.GetLastThirtyDayColorProductDataChart(shopperProductColorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        // here we donot need shoppe id
        [HttpGet("GetLastThirtyDayBestShoppersOfColorProduct/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayBestShoppersOfColorProductChart(int productId, int colorId)
        {
            var res = _shopperService.GetLastThirtyDayBestShoppersOfColorProductChart(productId, colorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }
        
        // we do not need shopperId here
        [HttpGet("GetBestShoppersOfColorProduct/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetBestShoppersOfColorProductChart(int productId, int colorId)
        {

            var res = _shopperService.GetBestShoppersOfColorProductChart(productId, colorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        //discount
        [HttpGet("GetLastTwentyDiscountDataOfShopperProductColor/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLastTwentyDiscountDataOfShopperProductColorChart(int productId, int colorId)
        {
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
            {
                return NotFound();
            }

            string shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
            {
                return NotFound();
            }


            var res = _discountService.GetLastTwentyDiscountDataOfShopperProductColorChart(shopperProductColorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }
    }
}
