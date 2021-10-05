using System;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class NewCommentViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Display(Name = "عنوان متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentTitle { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentText { get; set; }

        [Display(Name = "میزان رضایت مندی از محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 100, ErrorMessage = "لطفا عددی بین {1} تا {2} استفاده کنید.")]
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
    }
}