using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ProductDetailForShow
    {
        // product
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<string> ProductImages { get; set; }
        public string BrandName { get; set; }
        public int ShoppersCount { get; set; }
        public double ProductScore { get; set; }

        //colors
        public IEnumerable<Tuple<int, string>> Colors { get; set; }
    }
}
