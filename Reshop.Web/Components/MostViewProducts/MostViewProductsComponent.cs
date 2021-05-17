﻿using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;

namespace Reshop.Web.Components.MostViewProducts
{
    public class MostViewProductsComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public MostViewProductsComponent(IProductService productService)
        {
            _productService = productService;
        }


        public IViewComponentResult Invoke()
        {
            var products = _productService.GetProductsWithType(ProductTypes.All, SortTypes.MostViews, 20);
            return View("MostViewProducts", products);
        }
    }
}
