using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Entities.Category
{
    public class ChildCategory
    {
        public ChildCategory()
        {
            
        }

        [Key]
        public int ChildCategoryId { get; set; }

        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ChildCategoryTitle { get; set; }

        #region Relations

        public virtual ICollection<ChildCategoryToCategory> ChildCategoryToCategories { get; set; }
        public virtual ICollection<ProductToChildCategory> ProductToChildCategories { get; set; }
        public virtual ICollection<BrandToChildCategory> BrandToChildCategories { get; set; }

        #endregion
    }
}
