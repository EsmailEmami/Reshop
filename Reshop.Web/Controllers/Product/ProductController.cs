using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using System.Threading.Tasks;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Category;
using Reshop.Domain.DTOs.Product;

namespace Reshop.Web.Controllers.Product
{
    public class ProductController : Controller
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IShopperService _shopperService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IShopperService shopperService, ICategoryService categoryService)
        {
            _productService = productService;
            _shopperService = shopperService;
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region product detail

        [HttpGet]
        [Route("Product/{productName}/{sellerId}")]
        public async Task<IActionResult> ProductDetail(string productName, string sellerId)
        {
            if (sellerId == null)
            {
                return BadRequest();
            }

            if (!await _shopperService.IsShopperProductColorExistAsync(sellerId))
                return NotFound();

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

        #endregion

        #region redirect

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

        // redirect to product detail
        [HttpGet]
        public async Task<IActionResult> ProductRedirect(int productId)
        {
            if (productId == 0)
                return NotFound();


            var product = await _productService.GetBestSellerOfProductAsync(productId);

            if (product == null)
                return NotFound();

            return RedirectToAction("ProductDetail", "Product",
                new { productName = product.Item1.Replace(" ", "-"), sellerId = product.Item2 });
        }

        #endregion

        #region products 

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

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
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

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
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

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
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

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("Products/{type}")]
        [Route("Products/{type}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("Products/{type}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{brands}/{search}")]
        public async Task<IActionResult> GetProducts(string type , int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            if (!Fixer.EnumContainValue<ProductTypes>(type))
                return NotFound();


            if (brands != null && brands.ToLower() == "null")
                brands = null;

            if (search != null && search.ToLower() == "null")
                search = null;

            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetProductsWithPaginationAsync(type, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
            }

            return View(result);
        }

        #endregion

        #region compare

        [HttpGet]
        [Route("Compare/{products}")]
        public async Task<IActionResult> Compare(string products)
        {
            if (string.IsNullOrEmpty(products))
                return NotFound();

            var listProducts = Fixer.SplitToListInt(products);

            if (listProducts == null || !listProducts.Any())
                return NotFound();

            if (listProducts.Distinct().Count() != listProducts.Count)
            {
                string listProductString = listProducts.Distinct().ToList().ListToString(",");

                return RedirectToAction("Compare", new
                {
                    products = listProductString
                });
            }

            if (listProducts.Count > 4)
            {
                return NotFound();
            }

            var model = new List<ProductDataForCompareViewModel>();

            foreach (var productId in listProducts)
            {
                var data = await _productService.GetProductDataForCompareAsync(productId);

                if (data == null)
                    return NotFound();

                model.Add(data);
            }

            if (!model.Any())
                return NotFound();

            if (model.Select(c => c.Type).Distinct().Skip(1).Any())
                return BadRequest();


            ViewBag.ChildCategoryId = await _categoryService.GetChildCategoryIdOfProductAsync(model.First().ProductId);

            return View("Compare", model);
        }

        [HttpGet]
        [Route("AddProductToCompare/{childCategoryId}/{currentProducts}")]
        [Route("AddProductToCompare/{childCategoryId}/{currentProducts}/{pageId}/{sortBy}/{minPrice}/{maxPrice}")]
        [Route("AddProductToCompare/{childCategoryId}/{currentProducts}/{pageId}/{sortBy}/{minPrice}/{maxPrice}/{brands}/{search}")]
        public async Task<IActionResult> AddProductToCompare(int childCategoryId, string currentProducts, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            var listProducts = Fixer.SplitToListInt(currentProducts);

            if (listProducts == null || !listProducts.Any())
                return NotFound();

            ViewBag.CurrentProducts = listProducts.ListToString(","); ;

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

            ViewBag.SelectedMinPrice = 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
                ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : result.ProductsMinPrice;
            }

            return View(result);
        }

        #endregion
    }
}