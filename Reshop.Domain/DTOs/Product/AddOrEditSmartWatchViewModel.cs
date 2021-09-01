using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditSmartWatchViewModel
    {
        // product 

        public int ProductId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Description { get; set; }

        [Display(Name = "نام محصول واقعی از برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int OfficialBrandProductId { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsActive { get; set; }

        // --------------------------------------------------------------------------- ITEMS

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
        public int Weight { get; set; }

        [Display(Name = "مناسب برای")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SuitableFor { get; set; }

        [Display(Name = "نوع کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Application { get; set; }

        [Display(Name = "فرم صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayForm { get; set; }

        [Display(Name = " جنس شیشه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GlassMaterial { get; set; }

        [Display(Name = " جنس بدنه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CaseMaterial { get; set; }

        [Display(Name = "جنس بند ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MaterialStrap { get; set; }

        [Display(Name = " نوع قفل بند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string TypeOfLock { get; set; }

        //Display
        [Display(Name = "صفحه نمایش رنگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ColorDisplay { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool TouchDisplay { get; set; }

        [Display(Name = "اندازه صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(8, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public double DisplaySize { get; set; }

        [Display(Name = "رزولوشن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Resolution { get; set; }

        [Display(Name = "تراکم پیکسلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PixelDensity { get; set; }

        [Display(Name = " نوع صفحه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayType { get; set; }

        [Display(Name = "دیگر مشخصات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MoreInformationDisplay { get; set; }

        //Hardware 

        [Display(Name = " قابلیت نصب سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool SimcardIsSoppurt { get; set; }

        [Display(Name = " امکان ریجستر شدن سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool RegisteredSimCardIsSoppurt { get; set; }

        [Display(Name = "GPS")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool GpsIsSoppurt { get; set; }

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Os { get; set; }

        [Display(Name = "سازگاری با")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Compatibility { get; set; }

        [Display(Name = " پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Prossecor { get; set; }

        [Display(Name = " حافظه ی داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int InternalStorage { get; set; }

        [Display(Name = " قابلیت نصب کارت حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ExternalStorageSoppurt { get; set; }

        [Display(Name = "دوربین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Camera { get; set; }

        [Display(Name = "کنترل موسیقی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool MusicControl { get; set; }

        [Display(Name = "اتصالات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connections { get; set; }

        [Display(Name = "سنسور ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Sensors { get; set; }

        [Display(Name = "نوع باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BatteryMaterial { get; set; }

        [Display(Name = "قابلیت مکالمه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool CallIsSoppurt { get; set; }

        [Display(Name = "سایر مشخصات سخت افزاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string MoreInformationHardware { get; set; }

        // ---------------------------------------------------------------------------IMG

        [Display(Name = "عکس شماره 1")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage1 { get; set; }

        [Display(Name = "عکس شماره 2")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage2 { get; set; }

        [Display(Name = "عکس شماره 3")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage3 { get; set; }

        [Display(Name = "عکس شماره 4")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage4 { get; set; }

        [Display(Name = "عکس شماره 5")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage5 { get; set; }

        [Display(Name = "عکس شماره 6")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public IFormFile SelectedImage6 { get; set; }


        // for show on edit

        public string SelectedImage1IMG { get; set; }

        public string SelectedImage2IMG { get; set; }

        public string SelectedImage3IMG { get; set; }

        public string SelectedImage4IMG { get; set; }

        public string SelectedImage5IMG { get; set; }

        public string SelectedImage6IMG { get; set; }
    }
}
