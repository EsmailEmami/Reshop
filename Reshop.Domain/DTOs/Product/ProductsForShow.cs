using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductsForShow
    {
        public string Type { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Tuple<int, string, string>> Brands { get; set; }

        public decimal ProductsMinPrice { get; set; }
        public decimal ProductsMaxPrice { get; set; }
    }
}
