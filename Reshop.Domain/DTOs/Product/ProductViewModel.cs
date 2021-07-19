using System;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }
        public Tuple<byte, DateTime> Discount { get; set; }
        public string BrandName { get; set; }

        public string ShopperProductId { get; set; }
    }
}