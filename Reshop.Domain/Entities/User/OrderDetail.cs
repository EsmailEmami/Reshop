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

        [ForeignKey("ShopperProduct")]
        public string ShopperProductId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Display(Name = "قیمت کالا")]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "مقدار تخفیف محصول")]
        [Required]
        public byte ProductDiscountPercent { get; set; }

        [Display(Name = "تاریخ ثبت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime CreateDate { get; set; }

        public DateTime LastDataModifiedDate { get; set; } // price, discount...

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
        public virtual ShopperProduct ShopperProduct { get; set; }
        public Product.Product Product { get; set; }

        #endregion
    }
}
