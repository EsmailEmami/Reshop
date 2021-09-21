using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.Entities.Product
{
    public class Brand
    {
        public Brand()
        {
            
        }

        [Key]
        public int BrandId { get; set; }

        [ForeignKey("StoreTitle")]
        public int StoreTitleId { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandName { get; set; }

        public bool IsActive { get; set; }

        #region Relations

        public virtual ICollection<OfficialBrandProduct> OfficialBrandProducts { get; set; }
        public virtual StoreTitle StoreTitle { get; set; }

        #endregion
    }
}
