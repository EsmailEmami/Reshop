using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Entities.Shopper
{
    public class ShopperProductRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperProductRequestId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Shopper")]
        public string ShopperId { get; set; }

        [ForeignKey("User")]
        public string RequestUserId { get; set; }

        [ForeignKey("Color")]
        public int ColorId { get; set; }

        [Display(Name = "نوع درخواست")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool RequestType { get; set; } // true = add / false = edit

        [Display(Name = "گارانتی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Warranty { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public bool IsSuccess { get; set; }

        [Display(Name = "دلیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Reason { get; set; } = "Null";

        #region Relations

        public virtual User.User User { get; set; }
        public virtual Color Color { get; set; }
        public virtual Shopper Shopper { get; set; }
        public virtual Product.Product Product { get; set; }

        #endregion
    }
}