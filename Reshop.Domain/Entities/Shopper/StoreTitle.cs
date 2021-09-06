using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Entities.Shopper
{
    public class StoreTitle
    {
        public StoreTitle()
        {

        }

        [Key]
        public int StoreTitleId { get; set; }

        [Display(Name = "نام عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StoreTitleName { get; set; }

        #region Relations

        public virtual ICollection<ShopperStoreTitle> ShopperTitles { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }

        #endregion
    }
}
