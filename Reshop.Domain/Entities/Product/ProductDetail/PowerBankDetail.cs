using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class PowerBankDetail
    {
        [Key]
        public int PowerBankId { get; set; }

        [Display(Name = "وزن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Weight { get; set; }

        [Display(Name = "محدوده ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CapacityRange { get; set; }

        [Display(Name = "شدت جریان خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputCurrentIntensity { get; set; }

        [Display(Name = "تعداد درگاه خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public byte OutputPortsCount { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع کوالکام (QC)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfQCTechnology { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع Power Delivery (PD)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfPDTechnology { get; set; }

        [Display(Name = "قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }

        [Display(Name = "جنس بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BodyMaterial { get; set; }
    }
}
