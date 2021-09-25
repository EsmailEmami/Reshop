using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Shopper;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductsGeneralDataForAdmin
    {
        public ProductsGeneralDataForAdmin(int productsCount, int lastThirtyDaySellsCount, int activeProductsCount, int nonActiveProductsCount)
        {
            ProductsCount = productsCount;
            LastThirtyDaySellsCount = lastThirtyDaySellsCount;
            ActiveProductsCount = activeProductsCount;
            NonActiveProductsCount = nonActiveProductsCount;
        }

        public int ProductsCount { get; set; }
        public int LastThirtyDaySellsCount { get; set; }
        public int ActiveProductsCount { get; set; }
        public int NonActiveProductsCount { get; set; }
    }
}
