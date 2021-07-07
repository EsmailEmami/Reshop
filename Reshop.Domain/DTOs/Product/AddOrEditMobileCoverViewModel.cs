using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditMobileCoverViewModel
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

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int BrandProduct { get; set; }



        // --------------------------------------------------------------------------- ITEMS

        [Display(Name = "مناسب برای موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SuitablePhones { get; set; }

        [Display(Name = "جنس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Gender { get; set; }

        [Display(Name = "ساختار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Structure { get; set; }

        [Display(Name = "سطح پوشش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CoverLevel { get; set; }

        [Display(Name = "قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
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
