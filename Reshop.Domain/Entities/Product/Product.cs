using System;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Reshop.Domain.Entities.Product
{
    public class Product
    {
        public Product()
        {

        }

        [Key]
        public int ProductId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }

        [Required]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ShortKey { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1000, 1000000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد")]
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }

        [Display(Name = "نوع محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductType { get; set; }

        [Display(Name = "برند محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Brand { get; set; }

        [Display(Name = "نام محصول برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BrandProduct { get; set; }

        [Display(Name = "تعداد کل ویو ها")]
        public int AllViewsCount { get; set; }

        [Display(Name = "تعداد کل فروش ها")]
        public int AllSalesCount { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("MobileDetail")]
        public int? MobileDetailId { get; set; }

        [ForeignKey("LaptopDetail")]
        public int? LaptopDetailId { get; set; }

        [ForeignKey("MobileCoverDetail")]
        public int? MobileCoverDetailId { get; set; }

        [ForeignKey("TabletDetail")]
        public int? TabletDetailId { get; set; }

        [ForeignKey("SpeakerDetail")]
        public int? SpeakerDetailId { get; set; }

        [ForeignKey("PowerBankDetail")]
        public int? PowerBankDetailId { get; set; }

        [ForeignKey("WristWatchDetail")]
        public int? WristWatchDetailId { get; set; }

        [ForeignKey("SmartWatchDetail")]
        public int? SmartWatchDetailId { get; set; }

        [ForeignKey("HandsfreeAndHeadPhoneDetail")]
        public int? HandsfreeAndHeadPhoneDetailId { get; set; }

        [ForeignKey("FlashMemoryDetail")]
        public int? FlashMemoryDetailId { get; set; }

        [ForeignKey("BatteryChargerDetail")]
        public int? BatteryChargerDetailId { get; set; }

        [ForeignKey("MemoryCardDetail")]
        public int? MemoryCardDetailId { get; set; }

        #region Relations

        public virtual ICollection<ProductToChildCategory> ProductToChildCategories { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual MobileDetail MobileDetail { get; set; }

        public virtual LaptopDetail LaptopDetail { get; set; }

        public virtual ICollection<ProductGallery> ProductGalleries { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual MobileCoverDetail MobileCoverDetail { get; set; }

        public virtual TabletDetail TabletDetail { get; set; }

        public virtual SpeakerDetail SpeakerDetail { get; set; }

        public virtual PowerBankDetail PowerBankDetail { get; set; }

        public virtual WristWatchDetail WristWatchDetail { get; set; }

        public virtual SmartWatchDetail SmartWatchDetail { get; set; }

        public virtual HandsfreeAndHeadPhoneDetail HandsfreeAndHeadPhoneDetail { get; set; }

        public virtual FlashMemoryDetail FlashMemoryDetail { get; set; }

        public virtual BatteryChargerDetail BatteryChargerDetail { get; set; }

        public virtual MemoryCardDetail MemoryCardDetail { get; set; }

        public virtual ICollection<ShopperProduct> ShopperProducts { get; set; }
        public virtual ICollection<UserProductView> UserProductsView { get; set; }

        #endregion
    }
}
