using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.DTOs.Product
{
    public class CategoryOrChildCategoryProductsForShow
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int PageId { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<string> Brands { get; set; }

        
        public string ProductsMaxPrice { get; set; }
    }
}
