using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class TabletDetail
    {
        [Key]
        public int TabletDetailId { get; set; }

        //General specifications

        [Display(Name = "طول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Lenght { get; set; }

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
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Weight { get; set; }

        [Display(Name = "قابلیت نصب سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool SimCardIsTrue { get; set; }

        [Display(Name = "قابلیت تماس گرفتن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Call { get; set; }


        [Display(Name = "تعداد سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int SimCardQuantity { get; set; }


        [Display(Name = "ورودی سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SimCardInpute { get; set; }

        [Display(Name = "ورودی جداگانه برای حافظه جانبی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool SeparateSlotMemoryCard { get; set; }

        [Display(Name = "تاریخ معرفی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Announced { get; set; }

        //Chipset Information

        [Display(Name = "نام تراشه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ChipsetName { get; set; }

        [Display(Name = "پردازنده مرکزی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Cpu { get; set; }

        [Display(Name = "فرکانس پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuAndFrequency { get; set; }

        [Display(Name = "معماری پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuArch { get; set; }

        [Display(Name = " پردازنده گرافیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Gpu { get; set; }

        //Storage

        [Display(Name = "حافظه ی داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int InternalStorage { get; set; }

        [Display(Name = "Ram حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int Ram { get; set; }

        [Display(Name = "پشتیانی از حافظه ی جانبی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SdCard { get; set; }

        [Display(Name = "استاندارد حافظه جانبی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SdCardStandard { get; set; }

        //Display

        [Display(Name = "صفحه نمایش رنگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ColorDisplay { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool TouchDisplay { get; set; }

        [Display(Name = "فناوری صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayTechnology { get; set; }

        [Display(Name = "اندازه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplaySize { get; set; }

        [Display(Name = "رزولوشن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Resolution { get; set; }

        [Display(Name = "تراکم پیکسلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PixelDensity { get; set; }

        [Display(Name = "نسبت صفحه نمایش به بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(9, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ScreenToBodyRatio { get; set; }

        [Display(Name = "نسبت تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(9, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageRatio { get; set; }

        [Display(Name = "محافظت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(9, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayProtection { get; set; }

        [Display(Name = "سایر قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MoreInformation { get; set; }



        //Connections

        [Display(Name = "شبکه های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionsNetwork { get; set; }

        [Display(Name = "2G شبکه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GsmNetwork { get; set; }

        [Display(Name = "3G شبکه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string HspaNetwork { get; set; }

        [Display(Name = "4G شبکه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string LteNetwork { get; set; }

        [Display(Name = "5G شبکه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FiveGNetwork { get; set; }

        [Display(Name = "فناوری ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationTechnology { get; set; }

        [Display(Name = "Wi-Fi")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WiFi { get; set; }

        [Display(Name = "رادیو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Radio { get; set; }

        [Display(Name = "بلوتوث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Bluetooth { get; set; }

        [Display(Name = "فناوری مکان یابی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GpsInformation { get; set; }

        [Display(Name = "درگاه ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionPort { get; set; }


        //Camera

        [Display(Name = "تعداد لنز های دوربین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int CameraQuantity { get; set; }

        [Display(Name = "رزولوشن عکس دوربین اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int PhotoResolutation { get; set; }

        [Display(Name = "رزولوشن عکس دوربین سلفی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int SelfiCameraPhoto { get; set; }


        [Display(Name = "قابلیت های دوربین اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string CameraCapabilities { get; set; }

        [Display(Name = "قابلیت های دوربین سلفی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string SelfiCameraCapabilities { get; set; }

        [Display(Name = "فیلم برداری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Filming { get; set; }


        //Audio
        [Display(Name = "بلندگو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Speakers { get; set; }

        [Display(Name = "خروجی صدا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OutputAudio { get; set; }

        [Display(Name = "سایر امکانات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AudioInformation { get; set; }

        //Software

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OS { get; set; }

        [Display(Name = "نسخه ی سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(3, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int OsVersion { get; set; }

        [Display(Name = "رابط کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UiVersion { get; set; }

        [Display(Name = "سایر امکانات نرم افزاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string MoreInformationSoftWare { get; set; }

        //Battery

        [Display(Name = "نوع باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BatteryMaterial { get; set; }

        [Display(Name = "ظرفیت باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int BatteryCapacity { get; set; }

        [Display(Name = "باتری قابل تعویض")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool Removable‌Battery { get; set; }


        //MoreInformation
        [Display(Name = "سنسور ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Sensors { get; set; }

        [Display(Name = "اقلامات داخل جعبه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string ItemsInBox { get; set; }
    }
}
