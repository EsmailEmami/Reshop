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
    public class ShopperProductColor
    {
        public ShopperProductColor()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperProductColorId { get; set; }

        [ForeignKey("ShopperProduct")]
        public string ShopperProductId { get; set; }

        [ForeignKey("Color")]
        public int ColorId { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public decimal Price { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int QuantityInStock { get; set; }

        [Display(Name = "تعداد بازدید از محصول")]
        public int ViewCount { get; set; }

        [Display(Name = "تعداد فروش")]
        public int SaleCount { get; set; }

        #region Relations

        public virtual ShopperProduct ShopperProduct { get; set; }

        public virtual Color Color { get; set; }

        public virtual ICollection<ShopperProductDiscount> Discounts { get; set; }

        #endregion
    }
}
