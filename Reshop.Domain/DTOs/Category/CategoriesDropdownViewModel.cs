using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Category
{
    public class CategoriesDropdownViewModel
    {

        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        // first item is image name and the second item is url
        public IEnumerable<Tuple<string,string>> CategoryImages { get; set; }

        public IEnumerable<ChildCategoriesOfCategoryForDropDown> ChildCategories { get; set; }
    }

    public class ChildCategoriesOfCategoryForDropDown
    {
        public int ChildCategoryId { get; set; }
        public string ChildCategoryName { get; set; }

        // brandId, brand name, productsCount
        public IEnumerable<Tuple<int, string, int>> TopBrands { get; set; }
    }
}
