using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class SpeakerDetail
    {
        [Key]
        public int SpeakerDetailId { get; set; }

        [Display(Name = "نوع اتصال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ConnectionType { get; set; }

        [Display(Name = "اتصال دهنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "نسخه بلوتوٍث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double BluetoothVersion { get; set; }

        [Display(Name = "ورودی کارت حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsMemoryCardInput { get; set; }

        [Display(Name = "باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportBattery { get; set; }

        [Display(Name = "پورت USB")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportUSBPort { get; set; }

        [Display(Name = "میکروفون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportMicrophone { get; set; }

        [Display(Name = "رادیو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportRadio { get; set; }
    }
}
