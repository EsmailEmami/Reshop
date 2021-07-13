using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Shopper
{
    public class ShopperProduct
    {
        public ShopperProduct()
        {

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperProductId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Shopper")]
        public string ShopperId { get; set; }

        [Display(Name = "تعداد بازدید از محصول")]
        public int ViewCount { get; set; }

        [Display(Name = "تعداد فروش")]
        public int SaleCount { get; set; }

        [Display(Name = "گارانتی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Warranty { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public decimal Price { get; set; }

        [Display(Name = "درصد تخفیف")]
        public byte DiscountPercent { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }

        public bool IsFinally { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsInDiscount { get; set; }

        #region Relations


        public virtual Shopper Shopper { get; set; }

        public virtual Product.Product Product { get; set; }

        public ICollection<ShopperProductDiscount> Discounts { get; set; }

        #endregion
    }
}
