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

        [Display(Name = "مرتب سازی بر اساس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(1, 6, ErrorMessage = "{0} نمیتواند کوچکتر از {1} و بزرگتر از {2} باشد.")]
        public int OrderBy { get; set; }

        #region Relations

        public virtual Product Product { get; set; }

        #endregion
    }
}