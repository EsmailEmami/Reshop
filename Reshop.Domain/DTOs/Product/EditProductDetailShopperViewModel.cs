using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Product
{
    public class EditProductDetailShopperViewModel
    {
        public string ShopperId { get; set; }
        public string StoreName { get; set; }
        public string ShortKey { get; set; }
        public string SelectedShopper { get; set; }
        public int SelectedColor { get; set; }
        public decimal Price { get; set; }
        public string ProductTitle { get; set; }
        public Tuple<byte, DateTime> LastDiscount { get; set; }
        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }
        public IEnumerable<Tuple<int, string, string, string>> Colors { get; set; }
    }
}
