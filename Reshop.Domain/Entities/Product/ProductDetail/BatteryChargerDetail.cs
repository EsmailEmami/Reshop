using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class BatteryChargerDetail
    {
        [Key]
        public int BatteryChargerDetailId { get; set; }

        [Display(Name = "شدت جریان خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputCurrentIntensity { get; set; }

        [Display(Name = "تعداد درگاه خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public byte OutputPortsCount { get; set; }

        [Display(Name = "کابل همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MobileCable { get; set; }
    }
}
