using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class SpeakerDetail
    {
        [Key]
        public int SpeakerDetailId { get; set; }

        //General Information
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

        [Display(Name = "نوع اتصال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionType { get; set; }

        [Display(Name = "اتصال دهنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "ورودی کارت حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsMemoryCardInput { get; set; }

        [Display(Name = "پورت USB")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportUSBPort { get; set; }

        [Display(Name = "خروجی هدفون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool HeadphoneOutput{ get; set; }

        [Display(Name = "ورودی صدا ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool InputSound { get; set; }

        [Display(Name = "ورودی میکروفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool MicrophoneInpute { get; set; }

        [Display(Name = "میکروفون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportMicrophone { get; set; }

        [Display(Name = "صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Display { get; set; }

        [Display(Name = "ریموت کنترل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ControlRemote { get; set; }

        [Display(Name = "رادیو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportRadio { get; set; }

        [Display(Name = " بلوتوٍث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Bluetooth { get; set; }

        [Display(Name = "قابلیت اتصال به دو دستگاه به صورت همزمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ConnectTwoDevice { get; set; }

        //Speaker Information
        [Display(Name = "تعداد اجزای اسپیکر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int SpeakerItemQuantity { get; set; }

        [Display(Name = "باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsBattery { get; set; }

        [Display(Name = "مدت زمان پخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PlayingTime { get; set; }

        [Display(Name = "مدت زمان شارژ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ChargingTime { get; set; }

        //More Information

        [Display(Name = " سیستم عامل های قابل پشتیبانی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OsSoppurt { get; set; }







    }
}
