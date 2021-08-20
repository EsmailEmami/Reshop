using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductColorDetailViewModel
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int ReturnedCount { get; set; }
        public int SellCount { get; set; }
        public int LastMonthSellCount { get; set; }
        public decimal Income { get; set; }

        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}
