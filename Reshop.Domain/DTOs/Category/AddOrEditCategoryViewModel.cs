using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Entities.Category;

namespace Reshop.Domain.DTOs.Category
{
    public class AddOrEditCategoryViewModel
    {
        public int CategoryId { get; set; }

        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CategoryTitle { get; set; }

        public IEnumerable<int> SelectedChildCategories { get; set; }

        public IEnumerable<ChildCategory> ChildCategories { get; set; }
    }
}
