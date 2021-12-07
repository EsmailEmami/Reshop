using System;
using System.Collections.Generic;

namespace Reshop.Domain.DTOs.Category
{
    public class CategoriesMobileMenuViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public IEnumerable<ChildCategoriesOfCategoryForDropDown> ChildCategories { get; set; }
    }
}
