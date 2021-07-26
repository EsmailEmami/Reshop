using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Shopper
{
    public class ShopperProductDiscount
    {
        public ShopperProductDiscount()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShopperProductDiscountId { get; set; }

        [ForeignKey("ShopperProductColor")]
        public string ShopperProductColorId { get; set; }

        [Display(Name = "درصد تخفیف")]
        public byte DiscountPercent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #region Relations

        public virtual ShopperProductColor ShopperProductColor { get; set; }

        #endregion
    }
}
