using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditPowerBankViewModel
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
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Weight { get; set; }

        [Display(Name = "محدوده ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CapacityRange { get; set; }

        [Display(Name = " ولتاژ ورودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double InputVoltage { get; set; }

        [Display(Name = " ولتاژ خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputVoltage { get; set; }

        [Display(Name = " جریان ورودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double InputCurrentIntensity { get; set; }


        [Display(Name = " جریان خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double OutputCurrentIntensity { get; set; }

        [Display(Name = "تعداد درگاه خروجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int OutputPortsCount { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع کوالکام (QC)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfQCTechnology { get; set; }

        [Display(Name = "پشتیبانی از فناوری شارژ سریع Power Delivery (PD)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public bool IsSupportOfPDTechnology { get; set; }


        [Display(Name = "جنس بدنه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BodyMaterial { get; set; }

        [Display(Name = "نمایش شارژ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayCharge { get; set; }

        [Display(Name = "قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }

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
