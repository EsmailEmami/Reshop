using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Product
{
    public class CategoryOrChildCategoryProductsForShow
    {
        public int Id { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Tuple<int, string, string>> Brands { get; set; }


        public decimal ProductsMaxPrice { get; set; }
    }
}
