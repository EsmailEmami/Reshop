using Reshop.Domain.Entities.Category;
using Reshop.Domain.Entities.Product.ProductDetail;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Description { get; set; }

        [Display(Name = "نوع محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductType { get; set; }

        [ForeignKey("OfficialBrandProduct")]
        public int OfficialBrandProductId { get; set; }

        [ForeignKey("ChildCategory")]
        public int ChildCategoryId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }


        public bool IsActive { get; set; } = true;

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

        [ForeignKey("AuxDetail")]
        public int? AuxDetailId { get; set; }

        #region Relations

        public virtual OfficialBrandProduct OfficialBrandProduct { get; set; }

        public virtual ChildCategory ChildCategory { get; set; }

        public virtual ICollection<Comment.Comment> Comments { get; set; }

        public virtual MobileDetail MobileDetail { get; set; }

        public virtual LaptopDetail LaptopDetail { get; set; }

        public virtual ICollection<ProductGallery> ProductGalleries { get; set; }

        public virtual ICollection<Question.Question> Questions { get; set; }

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

        public virtual AUXDetail AuxDetail { get; set; }

        public virtual ICollection<ShopperProduct> ShopperProducts { get; set; }

        #endregion
    }
}
