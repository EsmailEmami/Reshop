using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class State
    {
        public State()
        {
        }

        [Key]
        public int StateId { get; set; }

        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StateName { get; set; }

        #region Relations

        public ICollection<City> Cities { get; set; }

        #endregion
    }
}
