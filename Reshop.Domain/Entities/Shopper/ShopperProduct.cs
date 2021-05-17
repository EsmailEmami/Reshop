using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Shopper
{
    public class ShopperProduct
    {
        public ShopperProduct()
        {

        }


         [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string ShopperUserId { get; set; }

        [Display(Name = "تعداد بازدید از محصول")]
        public int ViewCount { get; set; }

        [Display(Name = "تعداد فروش")]
        public int SaleCount { get; set; }

        #region Relations


        public virtual User.User User { get; set; }

        public virtual Product.Product Product { get; set; }

        #endregion
    }
}
