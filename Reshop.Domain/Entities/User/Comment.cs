using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.Entities.User
{
    public class Comment
    {
        public Comment()
        {
            
        }

        [Key]
        public int CommentId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("ShopperProductColor")]
        public string ShopperProductColorId { get; set; }

        [Display(Name = "عنوان متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentTitle { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentText { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime CommentDate { get; set; }

        [Display(Name = "میزان رضایت مندی از محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0,100,ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
        public int ProductSatisfaction { get; set; }

        [Display(Name = "کیفیت ساخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
        public int ConstructionQuality { get; set; }

        [Display(Name = "امکانات و قابلیت ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
        public int FeaturesAndCapabilities { get; set; }

        [Display(Name = "طراحی و ظاهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
        public int DesignAndAppearance { get; set; }

        [Display(Name = "امتیاز کلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
        public int OverallScore { get; set; }

        public bool IsDelete { get; set; } = false;

        [Display(Name = "دلیل حذف")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DeleteDescription { get; set; }

        #region Relations

        public virtual Product.Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual ShopperProductColor ShopperProductColor { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
        public virtual ICollection<CommentFeedback> CommentFeedBacks { get; set; }

        #endregion
    }
}
