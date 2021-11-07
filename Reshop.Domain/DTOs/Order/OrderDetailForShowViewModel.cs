using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class OrderDetailForShowViewModel
    {
        public string OrderDetailId { get; set; }
        public int ProductsCount { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImg { get; set; }
        public string Warranty { get; set; }
        public string ShopperId { get; set; }
        public string ShopperStoreName { get; set; }
        public string ColorName { get; set; }
        public string TrackingCode { get; set; }
    }
}
