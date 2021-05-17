using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class Discount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DiscountId { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1, 100, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} درصد باشد")]
        public byte DiscountPercent { get; set; }

        [Display(Name = "تعداد")]
        [Range(1, 30000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} باشد")]
        public short? UsableCount { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }


        #region relations

        public virtual ICollection<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion
    }
}
