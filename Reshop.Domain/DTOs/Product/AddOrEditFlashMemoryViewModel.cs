using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditFlashMemoryViewModel
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

        [Display(Name = "نام محصول واقعی از برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int OfficialBrandProductId { get; set; }

        // --------------------------------------------------------------------------- ITEMS

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

        [Display(Name = "جنس بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(60, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BodyMaterial { get; set; }

        [Display(Name = "رابط")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Connector { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Capacity { get; set; }

        [Display(Name = " LEDنشانگر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool Led { get; set; }

        [Display(Name = "مقاوم در برابر ضربه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsImpactResistance { get; set; }

        [Display(Name = "مقاوم در برابر آب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool WaterResistance { get; set; }

        [Display(Name = "مقاوم در برابر شوک و لرزش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool ShockResistance { get; set; }

        [Display(Name = "مقاوم در برابر  خاک ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool DustResistance { get; set; }

        [Display(Name = " ضدخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool AntiScratch { get; set; }

        [Display(Name = " ضد لک و اثرانگشت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool AntiStain { get; set; }

        [Display(Name = "سرعت استاندارد انتقال اطلاعات ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SpeedDataTransfer { get; set; }

        [Display(Name = "سرعت خواندن اطلاعات ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SpeedDataReading { get; set; }

        [Display(Name = "سازگاری با سیستم عامل های")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OsCompatibility { get; set; }

        [Display(Name = " سایر امکانات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MoreInformation { get; set; }

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
