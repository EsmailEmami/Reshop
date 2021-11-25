using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.Image
{
    public class ImagePlace
    {
        public ImagePlace()
        {
        }

        [Key]
        public int ImagePlaceId { get; set; }

        [Display(Name = "مکان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Place { get; set; }

        #region Relations

        public virtual IEnumerable<Image> Images { get; set; }

        #endregion
    }
}
