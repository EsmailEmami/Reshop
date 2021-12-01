using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Chart;

namespace Reshop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;
        private readonly IDiscountService _discountService;

        public ProductController(IProductService productService, IBrandService brandService, IColorService colorService, IDiscountService discountService)
        {
            _productService = productService;
            _brandService = brandService;
            _colorService = colorService;
            _discountService = discountService;
        }

        [HttpGet("GetLastThirtyDayProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductDataChart(int productId)
        {
            var res = _productService.GetLastThirtyDayProductDataChart(productId);

            var finalRes = res.GroupBy(c => c.Date)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Key,
                    SellCount = c.Sum(g => g.SellCount),
                    ViewCount = 10
                });

            if (!finalRes.Any())
                return NotFound();


            return new ObjectResult(finalRes);
        }

        [HttpGet("GetLastThirtyDayProductsDataChart")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductsDataChart()
        {
            var res = _productService.GetLastThirtyDayProductsDataChart();

            var finalRes = res.GroupBy(c => c.Date)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Key,
                    SellCount = c.Sum(g => g.SellCount),
                    ViewCount = 10
                });

            if (!finalRes.Any())
                return NotFound();


            return new ObjectResult(finalRes);
        }

        [HttpGet("GetColorsOfProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetColorsOfProductDataChart(int productId)
        {
            var res = _colorService.GetColorsOfProductDataChart(productId);

            var finalResult = res.GroupBy(c => c.Item1)
                .Select(c => new Tuple<string, int, int, int>(
                    c.Key,
                    10,
                    c.Sum(g => g.Item3),
                    5))
                .ToList();

            if (!finalResult.Any())
                return NotFound();


            return new ObjectResult(finalResult);
        }

        //color
        [HttpGet("GetLastThirtyDayColorProductData/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayColorProductDataChart(int productId, int colorId)
        {
            var res = _colorService.GetLastThirtyDayColorProductDataChart(productId, colorId);

            var finalRes = res.GroupBy(c => c.Date)
                .Select(c => new LastThirtyDayProductDataChart()
                {
                    Date = c.Key,
                    SellCount = c.Sum(g => g.SellCount),
                    ViewCount = 10
                });

            if (!finalRes.Any())
                return NotFound();


            return new ObjectResult(finalRes);
        }

        // store title
        [HttpGet("GetBrandsOfStoreTitle/{storeTitleId}")]
        public IActionResult GetBrandsOfStoreTitle(int storeTitleId)
        {
            var res = _brandService.GetBrandsOfStoreTitle(storeTitleId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }

        [HttpGet("GetBrandOfficialProducts/{brandId}")]
        public IActionResult GetBrandOfficialProducts(int brandId)
        {
            var res = _brandService.GetBrandOfficialProducts(brandId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }

        [HttpGet("GetChildCategoriesOfBrand/{brandId}")]
        public IActionResult GetChildCategoriesOfBrand(int brandId)
        {
            var res = _brandService.GetChildCategoriesOfBrand(brandId);

            if (res == null)
                return NotFound();

            var model = res
                .Select(c => new Tuple<int, string>(
                    c.ChildCategoryId,
                    $"{c.ChildCategoryTitle} - {c.IsActive.BoolToText("فعال")}"
                    ));

            return new ObjectResult(model);
        }

        [HttpGet("GetProductsOfOfficialProduct/{officialProductId}")]
        public IActionResult GetProductsOfOfficialProduct(int officialProductId)
        {
            var res = _brandService.GetProductsOfOfficialProduct(officialProductId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }

        //discount
        [HttpGet("GetLastTwentyDiscountDataOfProductColor/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastTwentyDiscountDataOfProductColorChart(int productId, int colorId)
        {
            var res = _discountService.GetLastTwentyDiscountDataOfProductColorChart(productId, colorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }
    }
}