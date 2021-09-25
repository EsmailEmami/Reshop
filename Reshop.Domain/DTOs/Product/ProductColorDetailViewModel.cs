using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductColorDetailViewModel
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int ReturnedCount { get; set; }
        public int SellCount { get; set; }
        public int LastMonthSellCount { get; set; }
    }
}
