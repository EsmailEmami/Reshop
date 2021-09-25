using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetLastThirtyDayProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductDataChart(int productId)
        {
            var res = _productService.GetLastThirtyDayProductDataChart(productId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        [HttpGet("GetLastThirtyDayProductsDataChart")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayProductsDataChart()
        {
            var res = _productService.GetLastThirtyDayProductsDataChart();

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }


        [HttpGet("GetColorsOfProductData/{productId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetColorsOfProductDataChart(int productId)
        {
            var res = _productService.GetColorsOfProductDataChart(productId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }

        //color
        [HttpGet("GetLastThirtyDayColorProductData/{productId}/{colorId}")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult GetLastThirtyDayColorProductDataChart(int productId, int colorId)
        {
            var res = _productService.GetLastThirtyDayColorProductDataChart(productId, colorId);

            if (res is null)
            {
                return NotFound();
            }

            return new ObjectResult(res);
        }


        // store title
        [HttpGet("GetBrandsOfStoreTitle/{storeTitleId}")]
        public IActionResult GetBrandsOfStoreTitle(int storeTitleId)
        {
            var res = _productService.GetBrandsOfStoreTitle(storeTitleId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }

        [HttpGet("GetBrandOfficialProducts/{brandId}")]
        public IActionResult GetBrandOfficialProducts(int brandId)
        {
            var res = _productService.GetBrandOfficialProducts(brandId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }

        [HttpGet("GetProductsOfOfficialProduct/{officialProductId}")]
        public IActionResult GetProductsOfOfficialProduct(int officialProductId)
        {
            var res = _productService.GetProductsOfOfficialProduct(officialProductId);

            if (res == null)
                return NotFound();

            return new ObjectResult(res);
        }
    }
}
