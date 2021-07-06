using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditMobileCoverViewModel
    {
        [Required]
        public string ShopperUserId { get; set; }

        public int ProductId { get; set; } 

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

        public IEnumerable<IFormFile> SelectedImages { get; set; }
    }
}
