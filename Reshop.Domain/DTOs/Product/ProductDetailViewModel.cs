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
        public ProductDataForDetailViewModel Product { get; set; }

        public IEnumerable<ProductGallery> ProductGalleries { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }
        public IEnumerable<Tuple<int, string, string,string>> Colors { get; set; }

    }

    public class ProductDataForDetailViewModel
    {
        public string ShopperProductColorId { get; set; }
        public int ProductId { get; set; }
        public Tuple<int,string> Brand { get; set; }
        public string Title { get; set; }
        public object Detail { get; set; }
        public decimal Price { get; set; }
        public Tuple<byte,DateTime> LastDiscount { get; set; }
        public int SelectedColor { get; set; }
    }
}