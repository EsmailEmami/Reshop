using System;

namespace Reshop.Domain.DTOs.Discount
{
    public class DiscountsForShowViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SellCount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal Income { get; set; }
    }
}