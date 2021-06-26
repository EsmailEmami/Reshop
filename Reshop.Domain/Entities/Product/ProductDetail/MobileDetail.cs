using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class MobileDetail
    {
        public MobileDetail()
        {
            
        }

        [Key]
        public int MobileDetailId { get; set; }

        [Display(Name = "حافظه داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string InternalMemory { get; set; }

        [Display(Name = "شبکه های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationNetworks { get; set; }

        [Display(Name = "دوربین پشت گوشی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BackCameras { get; set; }

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystem { get; set; }

        [Display(Name = "توضیحات سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SIMCardDescription { get; set; }

        [Display(Name = "مقدار رم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RAMValue { get; set; }

        [Display(Name = "رزولوشن عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PhotoResolution { get; set; }

        [Display(Name = "نسخه سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystemVersion { get; set; }

        [Display(Name = "فناوری صفحه ‌نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayTechnology { get; set; }

        [Display(Name = "ویژگی های موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }

        [Display(Name = "اندازه موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double Size { get; set; }

        [Display(Name = "تعداد سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public byte QuantitySIMCard { get; set; }

        // ساختار بدنه
        // زمان معرفی
        // شیار مجزا برای کارت حافظه دارد ندارد
        // مدل
    }
}
