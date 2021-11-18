using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.Entities.Shopper
{
   public class ShopperProductColorRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperProductColorRequestId { get; set; }

        [ForeignKey("ShopperProduct")]
        public string ShopperProductId { get; set; }

        [ForeignKey("User")]
        public string RequestUserId { get; set; }

        [ForeignKey("Color")]
        public int ColorId { get; set; }

        [Display(Name = "نوع درخواست")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool RequestType { get; set; } // true = add / false = edit

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
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Reason { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public bool IsActive { get; set; }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual ShopperProduct ShopperProduct { get; set; }
        public virtual Color Color { get; set; }

        #endregion
    }
}
