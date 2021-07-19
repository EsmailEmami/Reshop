using System;
using System.Collections.Generic;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDetailViewModel
    {
        public ShopperProduct Product { get; set; }

        public ShopperProductDiscount Discount { get; set; }

        public IEnumerable<ProductGallery> ProductGalleries { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }

        public string ProductType { get; set; }
    }
}