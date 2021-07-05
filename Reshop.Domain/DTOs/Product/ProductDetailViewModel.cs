using System;
using System.Collections.Generic;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDetailViewModel
    {
        public Entities.Product.Product Product { get; set; }

        public IAsyncEnumerable<ProductGallery> ProductGalleries { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }

        public string SelectedShopperId { get; set; }

        public string ProductType { get; set; }
    }
}