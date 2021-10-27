using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Enums.User;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.User;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Comment;

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

        [HttpGet]
        [Route("Category/{categoryId}/{categoryName}")]
        public async Task<IActionResult> ProductsOfCategory(int categoryId, string categoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
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
        public async Task<IActionResult> ProductsOfChildCategory(int childCategoryId, string childCategoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string brands = null)
        {
            var selectedBrands = Fixer.SplitToListInt(brands);

            if (selectedBrands == null)
                return NotFound();

            var result = await _productService.GetChildCategoryProductsWithPaginationAsync(childCategoryId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedBrands);


            ViewData["ChildCategoryName"] = childCategoryName;

            if (result == null)
            {

            }

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = selectedBrands;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpGet]
        [Route("Brand/{brandId}/{brandName}")]
        public async Task<IActionResult> ProductsOfBrand(int brandId, string brandName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", string officialBrandProducts = null)
        {
            var selectedOfficialBrandProducts = Fixer.SplitToListInt(officialBrandProducts);

            if (selectedOfficialBrandProducts == null)
                return NotFound();

            var result = await _productService.GetBrandProductsWithPaginationAsync(brandId, sortBy, pageId, 24, search, minPrice, maxPrice, selectedOfficialBrandProducts);

            ViewData["BrandName"] = brandName;


            ViewBag.SortBy = sortBy;
            ViewBag.SelectedOfficialBrandProducts = selectedOfficialBrandProducts;
            ViewBag.SearchText = search;

            ViewBag.SelectedMinPrice = !string.IsNullOrEmpty(minPrice) ? int.Parse(minPrice) : 0;
            ViewBag.SelectedMaxPrice = 0;

            if (result != null)
            {
                ViewBag.SelectedMaxPrice = !string.IsNullOrEmpty(maxPrice) ? int.Parse(maxPrice) : result.ProductsMaxPrice;
            }

            return View(result);
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs]
        public async Task<IActionResult> AddToFavoriteProduct(string shopperProductColorId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _productService.AddFavoriteProductAsync(userId, shopperProductColorId);

            return result switch
            {
                FavoriteProductResultType.Successful => Json(new { success = true, resultType = "Successful" }),
                FavoriteProductResultType.ProductReplaced => Json(new { success = true, resultType = "ProductReplaced" }),
                FavoriteProductResultType.NotFound => Json(new { success = false, resultType = "NotFound" }),
                FavoriteProductResultType.Available => Json(new { success = false, resultType = "Available" }),
                _ => Json(new { success = false, resultType = "NotFound" })
            };
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteProduct(string favoriteProductId)
        {
            var favoriteProduct = await _productService.GetFavoriteProductByIdAsync(favoriteProductId);
            if (favoriteProduct != null)
            {
                await _productService.RemoveFavoriteProductAsync(favoriteProduct);
                return Redirect("/");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
