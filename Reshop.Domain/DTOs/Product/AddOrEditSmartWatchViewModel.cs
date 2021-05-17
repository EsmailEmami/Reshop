using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditSmartWatchViewModel
    {
        public int ProductId { get; set; } = 0;

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Description { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1000, 1000000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد")]
        [RegularExpression("([0-9]+)", ErrorMessage = "لطفا عدد وارد کنید")]
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "لطفا عدد وارد کنید")]
        public int QuantityInStock { get; set; }

        [Display(Name = "برند محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductBrand { get; set; }

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandProduct { get; set; }

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

        public IEnumerable<IFormFile> SelectedImages { get; set; }
    }
}
