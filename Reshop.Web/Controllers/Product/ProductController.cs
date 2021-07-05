using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.Entities.Product;

namespace Reshop.Web.Controllers.Product
{
    public class ProductController : Controller
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Product/{productId}/{productName}")]
        [Route("Product/{productId}/{productName}/{sellerId}")]
        public async Task<IActionResult> ProductDetail(int productId, string productName, string sellerId = "")
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
            var result = await _productService.GetCategoryProductsWithPaginationAsync(categoryId, sortBy, pageId, 24, search, minPrice, maxPrice, brands);
            ViewData["CategoryOrChildCategoryTitleName"] = categoryName;

            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = brands;
            ViewBag.SearchText = search;

            return View("ProductsOfCategoryOrChildCategory", result);
        }


        [HttpGet]
        [Route("ChildCategory/{childCategoryId}/{childCategoryName}")]
        public async Task<IActionResult> ProductsOfChildCategory(int childCategoryId, string childCategoryName, int pageId = 1, string minPrice = null, string maxPrice = null, string search = null, string sortBy = "news", List<string> brands = null)
        {
            var result = await _productService.GetChildCategoryProductsWithPaginationAsync(childCategoryId, sortBy, pageId, 24, search, minPrice, maxPrice, brands);
            ViewData["CategoryOrChildCategoryTitleName"] = childCategoryName;


            ViewBag.SortBy = sortBy;
            ViewBag.SelectedBrands = brands;
            ViewBag.SearchText = search;

            return View("ProductsOfCategoryOrChildCategory", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteProduct(int productId, string shopperUserId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _productService.AddFavoriteProductAsync(userId, productId, shopperUserId);

            return result switch
            {
                FavoriteProductResultType.Successful => Json(new { success = true, resultType = "Successful" }),
                FavoriteProductResultType.ProductReplaced => Json(new { success = true, resultType = "ProductReplaced" }),
                FavoriteProductResultType.NotFound => Json(new { success = false, resultType = "NotFound" }),
                FavoriteProductResultType.Available => Json(new { success = false, resultType = "Available" }),
                _ => Json(new { success = false, resultType = "NotFound" })
            };
        }

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
