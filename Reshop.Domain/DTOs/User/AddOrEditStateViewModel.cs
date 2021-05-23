using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.User
{
    public class AddOrEditStateViewModel
    {
        public int StateId { get; set; } = 0;


        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StateName { get; set; }

        public IAsyncEnumerable<City> Cities { get; set; }

        public IEnumerable<City> SelectedCities { get; set; }
    }
}
