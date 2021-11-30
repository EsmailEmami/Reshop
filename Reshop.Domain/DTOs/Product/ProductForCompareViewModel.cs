using System;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductForCompareViewModel
    {
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Tuple<byte, DateTime> LastDiscount { get; set; }
    }
}
