using System;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductViewModel
    {
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }
        public Tuple<byte, DateTime> LastDiscount { get; set; }

        public string Image { get; set; }

        public string ShopperProductColorId { get; set; }
    }
}