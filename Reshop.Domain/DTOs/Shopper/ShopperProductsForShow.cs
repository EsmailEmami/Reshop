using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Product;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductsForShow
    {
        public string ShopperId { get; set; }
        public string StoreName { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Tuple<int, string, string>> Brands { get; set; }

        public decimal ProductsMaxPrice { get; set; }
        public decimal ProductsMinPrice { get; set; }
    }
}
