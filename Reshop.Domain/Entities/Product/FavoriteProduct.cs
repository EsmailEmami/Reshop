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

        [Display(Name = "آیدی فروشنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ShopperUserId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        #endregion
    }
}
