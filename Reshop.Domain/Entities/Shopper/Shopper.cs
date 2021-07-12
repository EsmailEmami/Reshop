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

        [ForeignKey("User")]
        public string UserId { get; set; }

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

        [Display(Name = "وضعیت فروشنده")] public bool IsFinally { get; set; }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual ICollection<ShopperProduct> ShopperProducts { get; set; }
        public ICollection<EditShopperProductRequest> EditShopperProducts { get; set; }
        public virtual ICollection<ShopperStoreTitle> ShopperTitles { get; set; }
        public virtual ICollection<StoreAddress> StoresAddress { get; set; }

        #endregion
    }
}
