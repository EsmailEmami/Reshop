using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Product
{
    public class BrandProduct
    {
        public BrandProduct()
        {
        }

        [Key]
        public int BrandProductId { get; set; }

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandProductName { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        #region Relations

        public virtual Brand Brand { get; set; }

        #endregion
    }
}
