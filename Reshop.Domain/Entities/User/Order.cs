using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class Order
    {
        public Order()
        {

        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("OrderAddress")]
        public string? OrderAddressId { get; set; }

        [Display(Name = "کد پیگیری")]
        [Required]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string TrackingCode { get; set; }

        [Display(Name = "تاریخ ثبت سفارش")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "مقدار تخفیف سفارش")]
        [Required]
        public decimal OrderDiscount { get; set; }

        [Display(Name = "هزینه حمل و نقل")]
        [Required]
        public decimal ShippingCost { get; set; }

        [Display(Name = "جمع فاکتور")]
        [Required]
        public decimal Sum { get; set; }

        [Display(Name = "وضعیت پرداخت")]
        [Required]
        public bool IsPayed { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PayDate { get; set; }

        [Display(Name = "وضعیت تحویل")]
        [Required]
        public bool IsReceived { get; set; }

        #region Relations


        public virtual User User { get; set; }
        public virtual OrderAddress OrderAddress { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
