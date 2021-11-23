using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Image
{
    public class Image
    {
        public Image()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ImageId { get; set; }

        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageName { get; set; }

        [Display(Name = "ادرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageUrl { get; set; }

        [ForeignKey("ImagePlace")]
        public int ImagePlaceId { get; set; }

        #region Relations

        public virtual ImagePlace ImagePlace { get; set; }

        #endregion
    }
}
