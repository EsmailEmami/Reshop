using Reshop.Domain.Entities.Category;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product
{
    public class ProductToChildCategory
    {
        public ProductToChildCategory()
        {
            
        }

        public int ProductId { get; set; }

        public int ChildCategoryId { get; set; }

        #region Relations

        public virtual Product Product { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        #endregion

    }
}
