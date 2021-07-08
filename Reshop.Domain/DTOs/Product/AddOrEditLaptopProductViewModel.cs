using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditLaptopProductViewModel
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

        [Display(Name = "برند محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Brand { get; set; }
 
        // --------------------------------------------------------------------------- ITEMS

        //General specifications

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
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Weight { get; set; }

        //CPU

        [Display(Name = "سازنده پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuCompany { get; set; }

        [Display(Name = "سری پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuSeries { get; set; }

        [Display(Name = "مدل پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuModel { get; set; }

        [Display(Name = "فرکانس پردازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuFerequancy { get; set; }

        [Display(Name = "Cache حافظه ی ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CpuCache { get; set; }

        //Ram
        [Display(Name = "حافظه رم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(3, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int RamStorage { get; set; }

        [Display(Name = "نوع حافظه رم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RamStorageTeachnology { get; set; }

        //InternalStorage

        [Display(Name = "ظرفیت حافظه ی داخل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int Storage { get; set; }

        [Display(Name = "نوع حافظه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StorageTeachnology { get; set; }

        [Display(Name = "مشخصات حافظه ی داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StorageInformation { get; set; }

        //GPU

        [Display(Name = "سازنده پردازنده گرافیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GpuCompany { get; set; }

        [Display(Name = "مدل پردازنده گرافیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string GpuModel { get; set; }

        [Display(Name = "  حافظه اختصاصی پردازنده گرافیکی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int GpuRam { get; set; }

        //Display

        [Display(Name = "اندازه ی صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplaySize { get; set; }

        [Display(Name = "نوع صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayTeachnology { get; set; }

        [Display(Name = "دقت صفحه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayResolutation { get; set; }

        [Display(Name = "نرخ پاسخگویی صفحه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int RefreshDisplay { get; set; }

        [Display(Name = "صفحه نمایش مات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool BlurDisplay { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool TouchDisplay { get; set; }

        //Possibilities
        [Display(Name = "درایو نوری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool DiskDrive { get; set; }

        [Display(Name = "اثرانگشت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool FingerTouch { get; set; }

        [Display(Name = "وبکم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Webcam { get; set; }

        [Display(Name = "کیبورد با نور پس زمینه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool BacklightKey { get; set; }

        [Display(Name = "مشخصات تاچ پد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string TouchPadInformation { get; set; }

        [Display(Name = " مودم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ModemInformation { get; set; }

        [Display(Name = "Wi-Fi")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Wifi { get; set; }

        [Display(Name = "بلوتوث")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Bluetooth { get; set; }

        [Display(Name = "VGA درگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool VgaPort { get; set; }

        [Display(Name = "HTMI درگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool HtmiPort { get; set; }

        [Display(Name = "Display درگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool DisplayPort { get; set; }

        [Display(Name = "Lan درگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool LanPort { get; set; }

        [Display(Name = "USB-C درگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool UsbCPort { get; set; }

        [Display(Name = "USB-3 پشتیبانی از ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Usb3Port { get; set; }

        [Display(Name = "USB-C تعداد درگاه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int UsbCQuantity { get; set; }

        [Display(Name = "USB تعداد درگاه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int UsbQuantity { get; set; }

        [Display(Name = "USB3 تعداد درگاه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int Usb3Quantity { get; set; }

        //Battery

        [Display(Name = " نوع باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BatteryMaterial { get; set; }

        [Display(Name = " شارژدهی باتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BatteryCharging { get; set; }

        [Display(Name = "توضیحات باتری ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BatteryInformation { get; set; }

        //MoreInformation

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Os { get; set; }

        [Display(Name = "طبقه بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Classification { get; set; }

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