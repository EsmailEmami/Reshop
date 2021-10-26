using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } 

        #region Relations

        public virtual Category Category { get; set; }
        public virtual ICollection<Product.Product> Products { get; set; }
        public virtual ICollection<BrandToChildCategory> BrandToChildCategories { get; set; }

        #endregion
    }
}
