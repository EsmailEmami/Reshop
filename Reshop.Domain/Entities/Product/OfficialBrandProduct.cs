using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product
{
    public class OfficialBrandProduct
    {
        public OfficialBrandProduct()
        {
        }

        [Key]
        public int OfficialBrandProductId { get; set; }

        [Display(Name = "نام رسمی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OfficialBrandProductName { get; set; }


        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        public bool IsActive { get; set; }

        #region Relations

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        #endregion
    }
}
