using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductColorDiscountDetailViewModel
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int DiscountsCount { get; set; }
        public int SellCount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal Income { get; set; }

        // table
        //start date, end date, sell count, income
        public IEnumerable<Tuple<DateTime,DateTime,int,decimal>> Discounts { get; set; }
    }
}
