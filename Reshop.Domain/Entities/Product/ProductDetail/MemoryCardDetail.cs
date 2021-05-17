using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class MemoryCardDetail
    {
        [Key]
        public int MemoryCardDetailId { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Capacity { get; set; }

        [Display(Name = "اندازه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Size { get; set; }

        [Display(Name = "سرعت استاندارد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")] 
        public string SpeedStandard { get; set; }

        [Display(Name = "مقاوم در برابر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ResistsAgainst { get; set; }
    }
}
