using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.Components.User
{
    public class UserFavoriteProductsComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public UserFavoriteProductsComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int pageId = 1, string sortBy = "news")
        {
            var products = await _productService.GetUserFavoriteProductsWithPagination(userId, sortBy, pageId, 24);

            ViewBag.SelectedSortBy = sortBy;
            ViewBag.UserId = userId;

            return View("/Views/Shared/Components/User/UserFavoriteProducts.cshtml", products);
        }
    }
}
