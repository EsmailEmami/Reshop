using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Product.ProductDetail
{
    public class WristWatchDetail
    {
        [Key]
        public int WristWatchDetailId { get; set; }


        [Display(Name = "GPS")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsSupportGPS { get; set; }

        [Display(Name = "صفحه نمایش لمسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsTouchScreen { get; set; }

        [Display(Name = "فرم صفحه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WatchForm { get; set; }

        // جنس بدنه
        // نوع قفل بند
    }
}
