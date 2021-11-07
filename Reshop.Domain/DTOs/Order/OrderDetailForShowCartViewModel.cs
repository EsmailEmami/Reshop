using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class OrderDetailForShowCartViewModel
    {
        public string TrackingCode { get; set; }
        public int ProductsCount { get; set; }
        public decimal ProductPrice { get; set; }
        public Tuple<byte, DateTime> Discount { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImg { get; set; }
        public string Warranty { get; set; }
        public string ShopperStoreName { get; set; }
        public string ColorName { get; set; }
    }
}
