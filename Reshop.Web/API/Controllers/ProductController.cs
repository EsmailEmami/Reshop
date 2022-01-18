using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Product;
using System;
using System.Linq;

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

        [HttpGet("[action]")]
        public IActionResult SearchFilterMenu(string filter = null)
        {
            if (string.IsNullOrEmpty(filter))
                return new JsonResult(null);

            var menu = _productService.SearchProducts(filter);

            return new JsonResult(menu);
        }


        [HttpGet("GetLastThirtyDayProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductDataChart(int productId)
        {
            var data = _productService.GetLastThirtyDayProductDataChart(productId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("GetLastThirtyDayProductsDataChart")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductsDataChart()
        {
            var data = _productService.GetLastThirtyDayProductsDataChart();

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        [HttpGet("GetColorsOfProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetColorsOfProductDataChart(int productId)
        {
            var data = _colorService.GetColorsOfProductDataChart(productId);

            if (data == null)
                return NotFound();

            return new ObjectResult(data);
        }

        //color
        [HttpGet("GetLastThirtyDayColorProductData/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayColorProductDataChart(int productId, int colorId)
        {
            var data = _colorService.GetLastThirtyDayColorProductDataChart(productId, colorId);

            if (data == null)
                return NotFound();


            return new ObjectResult(data);
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

        [HttpGet("GetCpusOfChipset/{chipsetId}")]
        public IActionResult GetCpusOfChipset(string chipsetId)
        {
            var data = _productService.GetCpusOfChipset(chipsetId);

            if (data == null)
                return NotFound();

            var convertedData = data.Select(c => new Tuple<string, string>(c.CpuId, c.CpuName));

            return new ObjectResult(convertedData);
        }

        [HttpGet("GetGpusOfChipset/{chipsetId}")]
        public IActionResult GetGpusOfChipset(string chipsetId)
        {
            var data = _productService.GetGpusOfChipset(chipsetId);

            if (data == null)
                return NotFound();

            var convertedData = data.Select(c => new Tuple<string, string>(c.GpuId, c.GpuName));

            return new ObjectResult(convertedData);
        }

        [HttpGet("GetOperatingSystemVersionsOfOperatingSystem/{operatingSystem}")]
        public IActionResult GetOperatingSystemVersionsOfOperatingSystem(string operatingSystem)
        {
            var data = _productService.GetOperatingSystemVersionsOfOperatingSystem(operatingSystem);

            if (data == null)
                return NotFound();

            var convertedData = data.Select(c => new Tuple<string, string>(c.OperatingSystemVersionId, c.OperatingSystemVersionName));

            return new ObjectResult(convertedData);
        }

        //discount
        [HttpGet("[action]")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastTwentyDiscountDataOfProductColor(int productId, int colorId)
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