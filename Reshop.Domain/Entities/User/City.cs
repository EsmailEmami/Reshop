using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Entities.User
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "اسم شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CityName { get; set; }

        #region Relations

        public ICollection<StateCity> StateCities { get; set; }

        #endregion
    }
}
