using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.Shopper
{
    public class EditShopperViewModel
    {
        public string UserId { get; set; }


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

        [Display(Name = "تصویر روی کارت ملی فروشنده")]
        public IFormFile OnNationalCardImage { get; set; }

        [Display(Name = "تصویر پشت کارت ملی فروشنده")]
        public IFormFile BackNationalCardImage { get; set; }

        [Display(Name = "جواز کسب و کار")]
        public IFormFile BusinessLicenseImage { get; set; }

        [Display(Name = "تصویر روی کارت ملی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string OnNationalCardImageName { get; set; }

        [Display(Name = "تصویر پشت کارت ملی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BackNationalCardImageName { get; set; }

        [Display(Name = "جواز کسب و کار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string BusinessLicenseImageName { get; set; }

        public IEnumerable<StoreTitle> StoreTitles { get; set; }
        public IEnumerable<int> SelectedStoreTitles { get; set; }
        public IEnumerable<State> States { get; set; }
    }
}
