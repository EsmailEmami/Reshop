using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Product
{
    public class ProductGallery
    {
        public ProductGallery()
        {
        }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Display(Name = "ایدی عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageName { get; set; }

        #region Relations

        public virtual Product Product { get; set; }

        #endregion
    }
}