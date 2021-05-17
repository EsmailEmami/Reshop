using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class LaptopDetail
    {
        public LaptopDetail()
        {
            
        }

        [Key]
        public int LaptopDetailId { get; set; }

        [Display(Name = "مقدار حافظه رم (GB)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RAMCapacity { get; set; }

        [Display(Name = "ظرفیت حافظه داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string InternalMemory { get; set; }

        [Display(Name = "سازنده پردازنده گرافیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GPUManufacturer { get; set; }

        [Display(Name = "اندازه صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Size { get; set; }

        [Display(Name = "طبقه بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Category { get; set; }

        [Display(Name = "سری پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProcessorSeries { get; set; }

        [Display(Name = "نوع حافظه RAM")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RAMType { get; set; }

        [Display(Name = "دقت صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ScreenAccuracy { get; set; }

        [Display(Name = "صفحه نمایش مات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsMatteScreen { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsTouchScreen { get; set; }

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystem { get; set; }

        [Display(Name = "پورت HDMI")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsHDMIPort { get; set; }
    }
}