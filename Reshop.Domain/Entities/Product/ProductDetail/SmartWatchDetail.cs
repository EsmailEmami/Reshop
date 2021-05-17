using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class SmartWatchDetail
    {
        [Key] 
        public int SmartWatchDetailId { get; set; }

        [Display(Name = "مناسب برای اقایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSuitableForMen { get; set; }

        [Display(Name = "مناسب برای خانم ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSuitableForWomen { get; set; }

        [Display(Name = "صفحه نمایش رنگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsScreenColorful { get; set; }

        [Display(Name = "قابلیت نصب سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSIMCardSupporter { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsTouchScreen { get; set; }

        [Display(Name = "امکان ریجستر شدن سیم‌کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportSIMCardRegister { get; set; }

        [Display(Name = "مناسب برای")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WorkSuggestion { get; set; }

        [Display(Name = "GPS")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportGPS { get; set; }

        [Display(Name = "فرم صفحه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WatchForm { get; set; }

        [Display(Name = "جنس بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BodyMaterial { get; set; }

        [Display(Name = "اتصالات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connections { get; set; }

        [Display(Name = "حسگر ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Sensors { get; set; }

        [Display(Name = "قابلیت مکالمه مستقیم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsDirectTalkable { get; set; }

        [Display(Name = "ثابلیت مکالمه از طریق بلوتوث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsTalkableWithBluetooth { get; set; }
    }
}
