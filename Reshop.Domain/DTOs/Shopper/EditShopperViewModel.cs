using System;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Attribute;
using Reshop.Domain.Entities.Permission;

namespace Reshop.Domain.DTOs.Shopper
{
    public class EditShopperViewModel
    {
        [Required]
        public string ShopperId { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NationalCode { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BirthDay { get; set; }  

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StoreName { get; set; }

        [Display(Name = "فروشنده فعال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsActive { get; set; }

        [Display(Name = "تصویر روی کارت ملی فروشنده")]
        [AllowFileSize(FileSize = 1.5, ErrorMessage = "لطفا حجم فایل بیشتر از 1.5 مگابایت نباشد.")]
        public IFormFile OnNationalCardImage { get; set; }

        [Display(Name = "جواز کسب و کار")]
        [AllowFileSize(FileSize = 1.5, ErrorMessage = "لطفا حجم فایل بیشتر از 1.5 مگابایت نباشد.")]
        public IFormFile BusinessLicenseImage { get; set; }

        [Display(Name = "تصویر روی کارت ملی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string OnNationalCardImageName { get; set; }

        [Display(Name = "جواز کسب و کار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BusinessLicenseImageName { get; set; }

        public IEnumerable<StoreTitle> StoreTitles { get; set; }
        public IEnumerable<int> SelectedStoreTitles { get; set; }

        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
