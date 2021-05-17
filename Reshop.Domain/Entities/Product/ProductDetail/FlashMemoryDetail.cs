using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class FlashMemoryDetail
    {
        [Key]
        public int FlashDetailId { get; set; }

        [Display(Name = "رابط")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double Capacity { get; set; }

        [Display(Name = "مقاوم در برابر ضربه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsImpactResistance { get; set; }

    }
}
