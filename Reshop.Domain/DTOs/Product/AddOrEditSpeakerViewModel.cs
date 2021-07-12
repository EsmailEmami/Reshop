using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditSpeakerViewModel
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

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsActive { get; set; }

        // --------------------------------------------------------------------------- ITEMS

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
        public bool HeadphoneOutput { get; set; }

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
