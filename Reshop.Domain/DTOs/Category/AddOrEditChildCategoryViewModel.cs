using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Category
{
    public class AddOrEditChildCategoryViewModel
    {
        public int ChildCategoryId { get; set; }

        [Display(Name = "نام زیر گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ChildCategoryTitle { get; set; }

        public bool IsActive { get; set; }

        public int SelectedCategory { get; set; }

        public IEnumerable<Entities.Category.Category> Categories { get; set; }
    }
}