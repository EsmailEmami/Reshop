using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product
{
    public class UserProductView
    {
        public UserProductView()
        {
        }

        [Key]
        public int UserProductViewId { get; set; }

        [Display(Name = "آی پی کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UserIPAddress { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        #region Relations

        public virtual Product Product { get; set; }

        #endregion

    }
}
