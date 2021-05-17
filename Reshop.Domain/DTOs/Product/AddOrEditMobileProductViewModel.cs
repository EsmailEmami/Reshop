using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditMobileProductViewModel
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
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }


        [Display(Name = "برند محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductBrand { get; set; }

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandProduct { get; set; }

        [Display(Name = "حافظه داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string InternalMemory { get; set; }

        [Display(Name = "شبکه های ارتباطی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommunicationNetworks { get; set; }

        [Display(Name = "دوربین پشت گوشی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BackCameras { get; set; }

        [Display(Name = "سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystem { get; set; }

        [Display(Name = "توضیحات سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SIMCardDescription { get; set; }

        [Display(Name = "مقدار رم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RAMValue { get; set; }

        [Display(Name = "رزولوشن عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PhotoResolution { get; set; }

        [Display(Name = "نسخه سیستم عامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OperatingSystemVersion { get; set; }

        [Display(Name = "فناوری صفحه ‌نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DisplayTechnology { get; set; }

        [Display(Name = "ویژگی های موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Features { get; set; }

        [Display(Name = "اندازه موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public double Size { get; set; }

        [Display(Name = "تعداد سیم کارت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public byte QuantitySIMCard { get; set; }

        public IEnumerable<IFormFile> SelectedImages { get; set; }

    }
}