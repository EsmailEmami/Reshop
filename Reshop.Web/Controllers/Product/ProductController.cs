using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.Controllers.Product
{
    public class ProductController : Controller
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Product/{productId}/{productName}")]
        public async Task<IActionResult> ProductDetail(int productId, string productName)
        {
            var product = await _productService.GetProductDetailAsync(productId);

            if (product == null)
                return NotFound();

            ViewData["ProductName"] = productName;

            return View(product);
        }

        [HttpGet]
        [Route("p/{key}")]
        public async Task<IActionResult> ShortKeyRedirect(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();


            var product = await _productService.GetProductByShortKeyAsync(key);

            if (product == null)
                return NotFound();


            return RedirectToAction("ProductDetail","Product",
                new { productId = product.ProductId, productName = product.ProductTitle});
        }

    }
}
