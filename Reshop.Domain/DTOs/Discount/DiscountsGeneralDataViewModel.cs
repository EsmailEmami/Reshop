using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Discount
{
    public class DiscountsGeneralDataViewModel
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int DiscountsCount { get; set; }
        public int SellCount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal Income { get; set; }
    }
}
