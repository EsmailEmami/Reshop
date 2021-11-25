using Reshop.Domain.DTOs.Product;
using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Category
{
    public class ChildCategoryProductsForShow
    {
        public int ChildCategoryId { get; set; }
        public string ChildCategoryName { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Tuple<int, string, string>> Brands { get; set; }
        public Tuple<int, string> Category { get; set; }

        public decimal ProductsMaxPrice { get; set; }
        public decimal ProductsMinPrice { get; set; }
    }
}
