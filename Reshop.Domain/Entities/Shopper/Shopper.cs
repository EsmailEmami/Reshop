using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Shopper
{
    public class Shopper
    {
        public Shopper()
        {

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperId { get; set; }


        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StoreName { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "تاریخ فروشنده شدن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime RegisterShopper { get; set; }

        [Display(Name = "شماره تلفن ثابت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string LandlinePhoneNumber { get; set; }

        [Display(Name = "تصویر روی کارت ملی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string OnNationalCardImageName { get; set; }

        [Display(Name = "تصویر پشت کارت ملی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BackNationalCardImageName { get; set; }

        [Display(Name = "جواز کسب و کار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BusinessLicenseImageName { get; set; }

        // درحال بررسی و بررسی شده
        [Display(Name = " وضعیت فروشنده")]
        [Required]
        public bool Condition { get; set; }

        // تایید شده و تایید نشده
        [Display(Name = "وضعیت نهایی")]
        [Required]
        public bool IsApproved { get; set; }

        #region Relations

        public virtual ICollection<ShopperProduct> ShopperProducts { get; set; }
        public virtual ICollection<ShopperStoreTitle> ShopperTitles { get; set; }

        #endregion
    }
}
