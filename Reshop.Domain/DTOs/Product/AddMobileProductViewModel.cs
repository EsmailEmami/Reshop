using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product.Options;
using Reshop.Domain.Entities.Shopper;
using OperatingSystem = Reshop.Domain.Entities.Product.Options.OperatingSystem;

namespace Reshop.Domain.DTOs.Product;

public class AddMobileProductViewModel
{
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

    public IEnumerable<StoreTitle> StoreTitles { get; set; }
    public IEnumerable<Tuple<int, string>> Brands { get; set; }
    public IEnumerable<Tuple<int, string>> OfficialProducts { get; set; }
    public IEnumerable<ChildCategory> ChildCategories { get; set; }

    public int SelectedStoreTitle { get; set; }
    public int SelectedBrand { get; set; }
    public int SelectedChildCategory { get; set; }

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

    [Display(Name = "تعداد سیم کارت")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Range(0, 2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int SimCardQuantity { get; set; }

    [Display(Name = "ورودی سیم کارت")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SimCardInput { get; set; }

    [Display(Name = "شیار کارت حافظه")]
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
    public string SelectedChipset { get; set; }

    [Display(Name = "پردازنده مرکزی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelectedCpu { get; set; }

    [Display(Name = "فرکانس پردازنده")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CpuAndFrequency { get; set; }

    [Display(Name = "معماری پردازنده")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelectedCpuArch { get; set; }

    [Display(Name = " پردازنده گرافیکی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelectedGpu { get; set; }

    //Storage

    [Display(Name = "حافظه ی داخلی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Range(0, 1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int InternalStorage { get; set; }

    [Display(Name = "Ram حافظه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [Range(0, 500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
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

    [Display(Name = "اندازه(اینچ)")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string DisplaySize { get; set; }

    [Display(Name = "رزولوشن")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string Resolution { get; set; }

    [Display(Name = "تراکم پیکسلی (اینچ)")]
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

    [Display(Name = "محافظت صفحه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string DisplayProtection { get; set; }

    [Display(Name = "سایر قابلیت ها")]
    [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string DisplayMoreInformation { get; set; }

    //Connections

    [Display(Name = "شبکه های ارتباطی")]
    [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> ConnectionsNetwork { get; set; }

    [Display(Name = "2G شبکه")]
    [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> GsmNetwork { get; set; }

    [Display(Name = "3G شبکه")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> HspaNetwork { get; set; }

    [Display(Name = "4G شبکه")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> LteNetwork { get; set; }

    [Display(Name = "5G شبکه")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> FiveGNetwork { get; set; }

    [Display(Name = "فناوری ارتباطی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CommunicationTechnology { get; set; }

    [Display(Name = "Wi-Fi")]
    [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> WiFi { get; set; }

    [Display(Name = "رادیو")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public bool Radio { get; set; }

    [Display(Name = "بلوتوث")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> Bluetooth { get; set; }

    [Display(Name = "فناوری مکان یابی")]
    [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public IEnumerable<string> GpsInformation { get; set; }

    [Display(Name = "درگاه ارتباطی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(70, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string ConnectionPort { get; set; }


    [Display(Name = "سایر قابلیت ها")]
    [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string ConnectionsMoreInformation { get; set; }


    //Camera

    [Display(Name = "دوربین ها")]
    public IEnumerable<string> Cameras { get; set; }

    [Display(Name = "رزولوشن عکس دوربین اصلی")]
    [Range(0, 100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int PhotoResolution { get; set; }

    [Display(Name = "رزولوشن عکس دوربین سلفی")]
    [Range(0, 100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int SelfiCameraResolution { get; set; }

    [Display(Name = "رزولوشن عکس دوربین ماکرو")]
    [Range(0, 100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int MacroCameraResolution { get; set; }

    [Display(Name = "رزولوشن عکس دوربین واید")]
    [Range(0, 100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int WideCameraResolution { get; set; }

    [Display(Name = "رزولوشن عکس دوربین عمق دید")]
    [Range(0, 100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public int DepthCameraResolution { get; set; }

    // camera Capabilities

    [Display(Name = "قابلیت های دوربین اصلی")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CameraCapabilities { get; set; }

    [Display(Name = "قابلیت های دوربین سلفی")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelfiCameraCapabilities { get; set; }

    [Display(Name = "قابلیت های دوربین ماکرو")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string MacroCameraCapabilities { get; set; }

    [Display(Name = "قابلیت های دوربین واید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string WideCameraCapabilities { get; set; }

    [Display(Name = "قابلیت های دوربین عمق دید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string DepthCameraCapabilities { get; set; }

    // video

    [Display(Name = "فیلم برداری دوربین اصلی")]
    public IEnumerable<string> PhotoCameraVideo { get; set; }

    [Display(Name = "فیلم برداری دوربین سلفی")]
    public IEnumerable<string> SelfiCameraVideo { get; set; }

    [Display(Name = "فیلم برداری دوربین ماکرو")]
    public IEnumerable<string> MacroCameraVideo { get; set; }

    [Display(Name = "فیلم برداری دوربین واید")]
    public IEnumerable<string> WideCameraVideo { get; set; }

    [Display(Name = "فیلم برداری دوربین عمق دید")]
    public IEnumerable<string> DepthCameraVideo { get; set; }

    [Display(Name = "سایر امکانات")]
    [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CameraMoreInformation { get; set; }

    //Audio
    [Display(Name = "بلندگو")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public bool Speakers { get; set; }

    [Display(Name = "خروجی صدا")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string OutputAudio { get; set; }

    [Display(Name = "سایر امکانات")]
    [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string AudioMoreInformation { get; set; }

    //Software

    [Display(Name = "سیستم عامل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelectedOS { get; set; }

    [Display(Name = "نسخه ی سیستم عامل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(40, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SelectedOsVersion { get; set; }

    [Display(Name = "رابط کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string UiVersion { get; set; }

    [Display(Name = "سایر امکانات نرم افزاری")]
    [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string SoftWareMoreInformation { get; set; }

    //Battery
     
    [Display(Name = "نوع باتری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string BatteryMaterial { get; set; }

    [Display(Name = "ظرفیت باتری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public int BatteryCapacity { get; set; }

    [Display(Name = "باتری قابل تعویض")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public bool Removable‌Battery { get; set; }


    //MoreInformation
    [Display(Name = "سنسور ها")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string Sensors { get; set; }

    [Display(Name = "اقلامات داخل جعبه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string ItemsInBox { get; set; }




    // --------------------------------------------------------------------------- LIST ITEMS

    public IEnumerable<Chipset> Chipsets { get; set; }
    public IEnumerable<Cpu> Cpus { get; set; }
    public IEnumerable<CpuArch> CpuArches { get; set; }
    public IEnumerable<Gpu> Gpus { get; set; }
    public IEnumerable<OperatingSystem> OperatingSystems { get; set; }
    public IEnumerable<OperatingSystemVersion> OperatingSystemVersions { get; set; }

    // ---------------------------------------------------------------------------IMG

    [Display(Name = "تصاویر کالا")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public List<IFormFile> Images { get; set; }
}