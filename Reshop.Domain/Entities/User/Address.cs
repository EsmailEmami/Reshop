using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class Address
    {
        public Address()
        {
            
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AddressId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string City { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string State { get; set; }

        [Display(Name = "پلاک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Plaque { get; set; }

        [Display(Name = "متن ادرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AddressText { get; set; }

        #region Relations

        public virtual User User { get; set; }

        #endregion
    }
}
