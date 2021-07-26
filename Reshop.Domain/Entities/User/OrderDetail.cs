using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.Entities.User
{
    public class OrderDetail
    {
        public OrderDetail()
        {

        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderDetailId { get; set; }

        [ForeignKey("Order")]
        public string OrderId { get; set; }

        [ForeignKey("ShopperProductColor")]
        public string ShopperProductColorId { get; set; }

        [Display(Name = "قیمت کالا")]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "مقدار تخفیف محصول")]
        [Required]
        public decimal ProductDiscountPrice { get; set; }

        [Display(Name = "تاریخ ثبت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "کد پیگیری")]
        [Required]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string TrackingCode { get; set; }

        [Display(Name = "تعداد کالا")]
        [Required]
        public int Count { get; set; }

        [Display(Name = "جمع کل")]
        [Required]
        public decimal Sum { get; set; }

        #region Relations


        public virtual Order Order { get; set; }
        public virtual ShopperProductColor ShopperProductColor { get; set; }

        #endregion
    }
}
