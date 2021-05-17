using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.Product;
using Reshop.Domain.Entities.Category;

namespace Reshop.Domain.DTOs.Category
{
    public class CategoriesDropdownViewModel
    {

        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        public IEnumerable<ChildCategory> ChildCategories { get; set; }

    }
}
