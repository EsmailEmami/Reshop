using Reshop.Domain.DTOs.CommentAndQuestion;
using Reshop.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using Reshop.Domain.Entities.Product.ProductDetail;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDetailViewModel
    {
        public ProductDataForDetailViewModel Product { get; set; }

        public IEnumerable<ProductGallery> ProductGalleries { get; set; }

        public CommentsOfProductDetailViewModel Comments { get; set; }

        public Tuple<int, string> Category { get; set; }

        public Tuple<int, string> ChildCategory { get; set; }

        public Tuple<string, string> Shopper { get; set; }

        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }
        public IEnumerable<Tuple<int, string, string, string>> Colors { get; set; }

    }

    public class ProductDataForDetailViewModel
    {
        public string ShopperProductColorId { get; set; }
        public string ShortKey { get; set; }
        public int ProductId { get; set; }
        public Tuple<int, string> Brand { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public Tuple<byte, DateTime> LastDiscount { get; set; }
        public int SelectedColor { get; set; }

        // Details 
        public MobileDetail MobileDetail { get; set; }
        public AUXDetail AuxDetail { get; set; }
        public LaptopDetail LaptopDetail { get; set; }
    }
}