using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("State")]
        public int StateId { get; set; }

        #region Relations

        public ICollection<State> State { get; set; }

        #endregion
    }
}
