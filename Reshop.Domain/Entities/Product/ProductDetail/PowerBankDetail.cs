using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class PowerBankDetail
    {
        [Key]
        public int PowerBankId { get; set; }


        [Display(Name = "طول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Length { get; set; }

        [Display(Name = "عرض")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Width { get; set; }

        [Display(Name = "ارتفاع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Height { get; set; }

        [Display(Name = "وزن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Weight { get; set; }

        [Display(Name = "محدوده ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CapacityRange { get; set; }

        [Display(Name = " ولتاژ ورودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double InputVoltage { get; set; }

        [Display(Name = " ولتاژ خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputVoltage { get; set; }

        [Display(Name = " جریان ورودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double InputCurrentIntensity { get; set; }


        [Display(Name = " جریان خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputCurrentIntensity { get; set; }

        [Display(Name = "تعداد درگاه خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int OutputPortsCount { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع کوالکام (QC)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfQCTechnology { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع Power Delivery (PD)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfPDTechnology { get; set; }


        [Display(Name = "جنس بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BodyMaterial { get; set; }

        [Display(Name = "نمایش شارژ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayCharge { get; set; }

        [Display(Name = "قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }
    }
}
