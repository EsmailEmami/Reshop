using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Category
{
    public class ChildCategoryToCategory
    {
        public ChildCategoryToCategory()
        {
            
        }

        public int ChildCategoryId { get; set; }

        public int CategoryId { get; set; }

        #region Relations

        public virtual Category Category { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        #endregion
    }
}
