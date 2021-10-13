using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditBrandViewModel
    {
        [Required]
        public int BrandId { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandName { get; set; }

        public bool IsActive { get; set; }

        public int SelectedStoreTitleId { get; set; }

        public IEnumerable<int> SelectedChildCategories { get; set; }

        public IEnumerable<StoreTitle> StoreTitles { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }
    }
}