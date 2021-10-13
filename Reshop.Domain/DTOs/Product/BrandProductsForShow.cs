using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Product
{
    public class BrandProductsForShow
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Tuple<int, string>> OfficialBrandProducts { get; set; }

        public decimal ProductsMaxPrice { get; set; }
    }
}