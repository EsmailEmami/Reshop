using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.Entities.Product
{
    public class FavoriteProduct
    {
        public FavoriteProduct()
        {

        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FavoriteProductId { get; set; }

        public string UserId { get; set; } 

        public int ProductId { get; set; }

       [ForeignKey("ShopperProduct")]
        public string ShopperProductId { get; set; }

        [ForeignKey("ShopperProductColor")]
        public string ShopperProductColorId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        public virtual ShopperProduct ShopperProduct { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ShopperProductColor ShopperProductColor { get; set; }

        #endregion
    }
}
