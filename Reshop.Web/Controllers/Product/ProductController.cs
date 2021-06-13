using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;

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


            return RedirectToAction("ProductDetail", "Product",
                new { productId = product.ProductId, productName = product.ProductTitle });
        }

        [HttpGet]
        [Route("Category/{categoryId}/{categoryName}")]
        public async Task<IActionResult> ProductsOfCategory(int categoryId, string categoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", List<string> brands = null)
        {
            var result = await _productService.GetCategoryProductsWithPaginationAsync(categoryId, sortBy, pageId, 24, search, minPrice, maxPrice,brands);
            ViewData["CategoryName"] = categoryName;

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = brands;

            return View(result);
        }
    }
}
