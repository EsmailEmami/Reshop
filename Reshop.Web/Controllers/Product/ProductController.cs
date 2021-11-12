using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
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
        [Route("Product/{productName}/{sellerId}")]
        public async Task<IActionResult> ProductDetail(string productName, string sellerId)
        {
            if (sellerId == null)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductDetailAsync(sellerId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // change seller 
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ChangeProductShopper(string seller)
        {
            if (string.IsNullOrEmpty(seller))
                return BadRequest();


            var product = await _productService.EditProductDetailShopperAsync(seller);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        [Route("p/{key}")]
        public async Task<IActionResult> ShortKeyRedirect(string key)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();


            var product = await _productService.GetProductRedirectionByShortKeyAsync(key);

            if (product == null)
                return NotFound();


            return RedirectToAction("ProductDetail", "Product",
                new { productName = product.Item1.Replace(" ", "-"), sellerId = product.Item2 });
        }

        [Route("Category/{categoryId}/{categoryName}")]
        [Route("Category/{categoryId}/{categoryName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("Category/{categoryId}/{categoryName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{brands}/{search}")]
        public async Task<IActionResult> ProductsOfCategory(int categoryId, string categoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            if (brands != null && brands.ToLower() == "null")
                brands = null;

            if (search != null && search.ToLower() == "null")
                search = null;

            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetCategoryProductsWithPaginationAsync(categoryId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);

            ViewData["CategoryName"] = categoryName;

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = minPrice.ToDecimal();
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}")]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{brands}/{search}")]
        public async Task<IActionResult> ProductsOfChildCategory(int childCategoryId, string childCategoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            if (brands != null && brands.ToLower() == "null")
                brands = null;

            if (search != null && search.ToLower() == "null")
                search = null;

            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetChildCategoryProductsWithPaginationAsync(childCategoryId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = minPrice.ToDecimal();
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? maxPrice.ToDecimal() : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("Brand/{brandId}/{brandName}")]
        [Route("Brand/{brandId}/{brandName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("Brand/{brandId}/{brandName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{officialBrandProducts}/{search}")]
        public async Task<IActionResult> ProductsOfBrand(int brandId, string brandName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string officialBrandProducts = null)
        {
            if (officialBrandProducts != null && officialBrandProducts.ToLower() == "null")
                officialBrandProducts = null;

            if (search != null && search.ToLower() == "null")
                search = null;

            var selectedOfficialBrandProducts = Fixer.SplitToListInt(officialBrandProducts);

            if (selectedOfficialBrandProducts == null)
                return NotFound();

            var result = await _productService.GetBrandProductsWithPaginationAsync(brandId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedOfficialBrandProducts);

            ViewData["BrandName"] = brandName;


            ViewBag.SortBy = sortBy;
            ViewBag.SelectedOfficialBrandProducts = selectedOfficialBrandProducts;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = minPrice.ToDecimal();
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? maxPrice.ToDecimal() : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("Shopper/{shopperId}/{storeName}")]
        [Route("Shopper/{shopperId}/{storeName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("Shopper/{shopperId}/{storeName}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{brands}/{search}")]
        public async Task<IActionResult> ProductsOfShopper(string shopperId, string storeName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            if (brands != null && brands.ToLower() == "null")
                brands = null;

            if (search != null && search.ToLower() == "null")
                search = null;

            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetShopperProductsWithPaginationAsync(shopperId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = minPrice.ToDecimal();
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? maxPrice.ToDecimal() : result.ProductsMaxPrice;
            }

            return View(result);
        }
    }
}
