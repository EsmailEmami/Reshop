using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Category
{
    public class ChildCategoriesOfCategoryForDropDown
    {
        public int ChildCategoryId { get; set; }
        public string ChildCategoryName { get; set; }

        // brandId, brand name, productsCount
        public IEnumerable<Tuple<int, string, int>> TopBrands { get; set; }
    }
}
