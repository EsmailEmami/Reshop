using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Product
{
    public class EditProductDetailShopperViewModel
    {
        public string SelectedShopper { get; set; }
        public int SelectedColor { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<Tuple<string, string, string>> Shoppers { get; set; }
        public IEnumerable<Tuple<int, string, string, string>> Colors { get; set; }
    }
}
