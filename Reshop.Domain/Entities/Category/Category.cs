using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Category
{
    public class Category
    {
        public Category()
        {
            
        }

        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CategoryTitle { get; set; }

        #region Relations

        public virtual ICollection<ChildCategoryToCategory> ChildCategoryToCategories { get; set; }

        #endregion
    }
}
