using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditMemoryCardViewModel
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

        public IEnumerable<StoreTitle> StoreTitles { get; set; }
        public IEnumerable<Tuple<int, string>> Brands { get; set; }
        public IEnumerable<Tuple<int, string>> OfficialProducts { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public int SelectedStoreTitle { get; set; }
        public int SelectedBrand { get; set; }
        public int SelectedChildCategory { get; set; }

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

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Capacity { get; set; }

        [Display(Name = "استاندارد سرعت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string SpeedStandard { get; set; }

        [Display(Name = "سرعت خواندن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public int ReadingSpeed { get; set; }

        [Display(Name = "مقاوم در برابر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ResistsAgainst { get; set; }

        [Display(Name = "سایر مشخصات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
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
